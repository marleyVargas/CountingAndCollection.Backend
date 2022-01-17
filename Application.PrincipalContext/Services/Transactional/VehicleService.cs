using Application.PrincipalContext.Interfaces.Application;
using Application.PrincipalContext.Interfaces.Orchestrator;
using Application.PrincipalContext.Interfaces.Transactional;
using Domain.Nucleus.Entities.Application;
using Domain.Nucleus.Entities.Orchestrator;
using Domain.Nucleus.Entities.Transactional;
using Domain.Nucleus.Exceptions;
using Domain.Nucleus.Interfaces;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Transversal.DTOs.Orchestrator;
using Transversal.DTOs.Transactional;
using Transversal.QueryFilters;
namespace Application.PrincipalContext.Services.OrchestratorServices
{
    public class VehicleService : IVehicleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IOrchestratorAPIService _orchestratorAPIService;
        private readonly ILogRequestAndResponseService _logRequestAndResponseService;

        public VehicleService(IUnitOfWork unitOfWork, ILogRequestAndResponseService logRequestAndResponseService, IOrchestratorAPIService orchestratorAPIService)
        {
            this._unitOfWork = unitOfWork;
            this._logRequestAndResponseService = logRequestAndResponseService;
            this._orchestratorAPIService = orchestratorAPIService;
        }

        public async Task<List<Collection>> GetVehicleCollectionByFilter(CollectionQueryFilter filters)
        {
            Expression<Func<Collection, bool>> query = q =>
            (
                q.CreatedDate >= filters.CreatedDateInit
                && q.CreatedDate <= filters.CreatedDateFin
            );

            var parameters = this._unitOfWork.CollectionRepository.GetByFilter(query).ToList();

            return parameters;
        }

        public async Task<bool> SaveVehicleCounting(DateTime consultationDate)
        {
            try
            {
                var responseCountingDto = JsonConvert.DeserializeObject<ResponseCountingDto>(await this._orchestratorAPIService.VehicleCounting(consultationDate));

                if (responseCountingDto == null)
                    throw new BusinessException("Invalid Vehicle Counting request");

                if (!this._unitOfWork.IsOnTransaction())
                    this._unitOfWork.BeginTransaction();

                var collectionBD = new Collection
                {
                    Amount = responseCountingDto.Cantidad,
                    Category = responseCountingDto.Categoria,
                    Direction = responseCountingDto.Sentido,
                    Station = responseCountingDto.Estacion,
                    Hour = responseCountingDto.Hora                    
                };

                await this._unitOfWork.CollectionRepository.Add(collectionBD);
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

        public async Task<bool> SaveVehicleCollection(DateTime consultationDate)
        {
            try
            {
                var responseCountingDto = JsonConvert.DeserializeObject<ResponseCollectionDto>(await this._orchestratorAPIService.VehicleCollection(consultationDate));

                if (responseCountingDto == null)
                    throw new BusinessException("Invalid Vehicle Collection request");

                if (!this._unitOfWork.IsOnTransaction())
                    this._unitOfWork.BeginTransaction();

                var collectionBD = new Collection
                {
                    TabulatedValue = responseCountingDto.ValorTabulado,
                    Category = responseCountingDto.Categoria,
                    Direction = responseCountingDto.Sentido,
                    Station = responseCountingDto.Estacion,
                    Hour = responseCountingDto.Hora
                };

                await this._unitOfWork.CollectionRepository.Add(collectionBD);
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
