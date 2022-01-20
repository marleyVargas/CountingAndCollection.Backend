using Application.PrincipalContext.Interfaces.Application;
using Application.PrincipalContext.Interfaces.Orchestrator;
using Application.PrincipalContext.Interfaces.Transactional;
using Application.PrincipalContext.Services.OrchestratorServices;
using AutoMapper;
using Domain.Nucleus.CustomEntities;
using Domain.Nucleus.Entities.Transactional;
using Domain.Nucleus.Exceptions;
using Domain.Nucleus.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Transversal.DTOs.Transactional;
using Transversal.DTOs.Transactional.Response;
using Transversal.QueryFilters;
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

        public async Task<PagedList<Collection>> GetVehicleCollectionByFilter(CollectionQueryFilter filters)
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

            return pagedCollections;
        }

        public async Task<List<Collection>> GetVehicleCollectionByDates(DateTime dateInit, DateTime dateEnd)
        {
            Expression<Func<Collection, bool>> query = q =>
           (
               q.QueryDate.Date >= dateInit.Date
               && q.QueryDate.Date <= dateEnd.Date
           );

            return this._unitOfWork.CollectionRepository.GetByFilter(query).ToList();
        }

        public async Task<bool> SaveVehicleCounting(DateTime queryDate)
        {
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

                return true;
            }
            catch
            {
                if (this._unitOfWork.IsOnTransaction())
                    this._unitOfWork.RollbackTransaction();

                throw;
            }

        }

        public async Task<bool> SaveVehicleCollection(DateTime queryDate)
        {
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

                return true;
            }
            catch
            {
                if (this._unitOfWork.IsOnTransaction())
                    this._unitOfWork.RollbackTransaction();

                throw;
            }

        }
    }
}
