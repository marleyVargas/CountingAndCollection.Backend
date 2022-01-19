using Application.PrincipalContext.Interfaces.Application;
using Application.PrincipalContext.Interfaces.Orchestrator;
using Application.PrincipalContext.Interfaces.Transactional;
using Application.PrincipalContext.Services.OrchestratorServices;
using AutoMapper;
using Domain.Nucleus.Entities.Transactional;
using Domain.Nucleus.Exceptions;
using Domain.Nucleus.Interfaces;
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

        public VehicleService(IUnitOfWork unitOfWork, ILogRequestAndResponseService logRequestAndResponseService, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._logRequestAndResponseService = logRequestAndResponseService;
            this._mapper = mapper;
        }

        public async Task<List<Collection>> GetVehicleCollectionByFilter(CollectionQueryFilter filters)
        {
            Expression<Func<Collection, bool>> query = q =>
            (
                q.QueryDate.Date >= filters.CreatedDateInit.Date
                && q.QueryDate.Date <= filters.CreatedDateFin.Date
            );

            return this._unitOfWork.CollectionRepository.GetByFilter(query).ToList();
        }

        public async Task<bool> SaveVehicleCounting(DateTime queryDate)
        {
            try
            {
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
            catch(Exception ex)
            {
                if (this._unitOfWork.IsOnTransaction())
                    this._unitOfWork.RollbackTransaction();

                throw;
            }

        }
    }
}
