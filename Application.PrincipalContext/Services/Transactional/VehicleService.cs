using Application.PrincipalContext.Interfaces.Application;
using Application.PrincipalContext.Interfaces.Orchestrator;
using Application.PrincipalContext.Interfaces.Transactional;
using Application.PrincipalContext.Services.OrchestratorServices;
using AutoMapper;
using Domain.Nucleus.CustomEntities;
using Domain.Nucleus.Entities.Transactional;
using Domain.Nucleus.Exceptions;
using Domain.Nucleus.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Transversal.DTOs;
using Transversal.DTOs.Transactional;
using Transversal.DTOs.Transactional.Response;
using Transversal.QueryFilters;
using Transversal.Response;

namespace Application.PrincipalContext.Services.TransactionalServices
{
    public class VehicleService : IVehicleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IOrchestratorAPIService _orchestratorAPIService;
        private ILogRequestAndResponseService _logRequestAndResponseService;
        private readonly IMapper _mapper;
        private readonly PaginationOptions _paginationOptions;

        public VehicleService(IUnitOfWork unitOfWork, ILogRequestAndResponseService logRequestAndResponseService, IMapper mapper, IOptions<PaginationOptions> options)
        {
            this._unitOfWork = unitOfWork;
            this._logRequestAndResponseService = logRequestAndResponseService;
            this._mapper = mapper;
            this._paginationOptions = options.Value;
        }

        public async Task<Response<ResponseCollectionPaginatorDto>> GetVehicleCollectionByFilter(CollectionQueryFilter filters)
        {
            Response<ResponseCollectionPaginatorDto> result = new();
            try
            {
                Expression<Func<Collection, bool>> query = q =>
                (
                    filters.CreatedDateInit.HasValue ? q.QueryDate.Date >= filters.CreatedDateInit.Value.Date : q.QueryDate.Date == q.QueryDate.Date
                    && filters.CreatedDateFin.HasValue ? q.QueryDate.Date >= filters.CreatedDateFin.Value.Date : q.QueryDate.Date == q.QueryDate.Date
                    && !String.IsNullOrEmpty(filters.Station) ? q.Station == filters.Station : q.Station == q.Station
                );

                filters.PageNumber = filters.PageNumber == 0 ? this._paginationOptions.DefaultPageNumber : filters.PageNumber;
                filters.PageSize = filters.PageSize == 0 ? this._paginationOptions.DefaultPageSize : filters.PageSize;

                var collections = this._unitOfWork.CollectionRepository.GetByFilter(query).ToList();

                var pagedCollections = PagedList<Collection>.Create(collections, filters.PageNumber, filters.PageSize);

                var collectionsDto = this._mapper.Map<List<ResponseVehicleCollectionDto>>(pagedCollections).ToList();

                var metadata = new Metadata
                {
                    TotalCount = pagedCollections.TotalCount,
                    PageSize = pagedCollections.PageSize,
                    CurrentPage = pagedCollections.CurrentPage,
                    TotalPages = pagedCollections.TotalPages,
                    HasNextPage = pagedCollections.HasNextPage,
                    HasPreviousPage = pagedCollections.HasPreviousPage
                };

                result.Result = new ResponseCollectionPaginatorDto
                {
                    data = collectionsDto,
                    meta = metadata
                };
            }
            catch (Exception ex)
            {
                result.ErrorProvider.AddError(ex.Source, ex.GetBaseException().Message);
            }
            return result;

        }

        public async Task<Response<object>> GetVehicleCollectionByDates(DateTime dateInit, DateTime dateEnd)
        {
            Response<object> result = new();
            try
            {
                Expression<Func<Collection, bool>> query = q =>
               (
                   q.QueryDate.Date >= dateInit.Date
                   && q.QueryDate.Date <= dateEnd.Date
                );

                var vehicles = this._unitOfWork.CollectionRepository.GetByFilter(query).ToList();
                var listDates = vehicles
                                .GroupBy(l => l.Station)
                                .Select(cl => new
                                {
                                    Station = cl.Key,
                                    DStation = cl.GroupBy(x => x.QueryDate)
                                            .Select(
                                                csLine => new
                                                {
                                                    Date = csLine.Key,
                                                    Quantity = csLine.Sum(c => c.Amount),
                                                    Value = csLine.Sum(c => c.TabuledValue)
                                                }).ToList(),
                                    Totals = cl.GroupBy(x => x.QueryDate)
                                            .Select(
                                                csLine => new
                                                {
                                                    TotalsQuantity = cl.Sum(c => c.Amount),
                                                    TotalsValue = cl.Sum(c => c.TabuledValue)
                                                }).FirstOrDefault(),
                                }).ToList();

                var totals = new
                {
                    TotalsQuantity = listDates.Sum(x => x.Totals.TotalsQuantity),
                    TotalsValue = listDates.Sum(x => x.Totals.TotalsValue)
                };

                result.Result = new
                {
                    listDates,
                    totals
                };                
            }
            catch (Exception ex)
            {
                result.ErrorProvider.AddError(ex.Source, ex.GetBaseException().Message);
            }
            return result;
        }

        public async Task<Response<bool>> SaveVehicleCounting(DateTime queryDate)
        {
            Response<bool> result = new();
            try
            {
                Expression<Func<Collection, bool>> query = q =>
                (
                    q.QueryDate.Date == queryDate && q.Amount > 0
                );
                var existDate = this._unitOfWork.CollectionRepository.GetByFilter(query).Any();

                if (existDate)
                    return false;

                var parameters = this._unitOfWork.ParametersRepository.GetAll();
                var url = parameters.Where(x => x.Code == "UrlOrchestrator").FirstOrDefault().Value;

                this._orchestratorAPIService = new OrchestratorAPIService(_unitOfWork, _logRequestAndResponseService, url);
                var listCountingDto = JsonConvert.DeserializeObject<List<CollectionDto>>(await this._orchestratorAPIService.VehicleCounting(queryDate));

                foreach(var item in listCountingDto)
                {
                    item.Fecha = queryDate.Date;
                }

                var responseCountingDto = this._mapper.Map<Collection[]>(listCountingDto);

                if (!this._unitOfWork.IsOnTransaction())
                    this._unitOfWork.BeginTransaction();

                await this._unitOfWork.CollectionRepository.AddRange(responseCountingDto);
                await this._unitOfWork.SaveChangesAsync();

                if (this._unitOfWork.IsOnTransaction())
                {
                    await this._unitOfWork.CommitTransactionAsync();
                }

                result.Result = true;
            }
            catch(Exception ex)
            {
                if (this._unitOfWork.IsOnTransaction())
                    this._unitOfWork.RollbackTransaction();

                result.ErrorProvider.AddError(ex.Source, ex.GetBaseException().Message); ;
                result.Result = false;
            }
            return result;
        }

        public async Task<Response<bool>> SaveVehicleCollection(DateTime queryDate)
        {
            Response<bool> result = new();
            try
            {
                Expression<Func<Collection, bool>> query = q =>
                  (
                      q.QueryDate.Date == queryDate && q.TabuledValue > 0
                  );
                var existDate = this._unitOfWork.CollectionRepository.GetByFilter(query).Any();

                if (existDate)
                    return false;

                var parameters = this._unitOfWork.ParametersRepository.GetAll();
                var url = parameters.Where(x => x.Code == "UrlOrchestrator").FirstOrDefault().Value;

                this._orchestratorAPIService = new OrchestratorAPIService(_unitOfWork, _logRequestAndResponseService, url);
                var listCountingDto = JsonConvert.DeserializeObject<List<CollectionDto>>(await this._orchestratorAPIService.VehicleCollection(queryDate));

                foreach (var item in listCountingDto)
                {
                    item.Fecha = queryDate.Date;
                }

                var responseCountingDto = this._mapper.Map<Collection[]>(listCountingDto);

                if (!this._unitOfWork.IsOnTransaction())
                    this._unitOfWork.BeginTransaction();

                await this._unitOfWork.CollectionRepository.AddRange(responseCountingDto);
                await this._unitOfWork.SaveChangesAsync();


                if (this._unitOfWork.IsOnTransaction())
                {
                    await this._unitOfWork.CommitTransactionAsync();
                }

                result.Result = true;
            }
            catch(Exception ex)
            {
                if (this._unitOfWork.IsOnTransaction())
                    this._unitOfWork.RollbackTransaction();

                result.ErrorProvider.AddError(ex.Source, ex.GetBaseException().Message); ;
                result.Result = false;
            }
            return result;
        }
    }
}
