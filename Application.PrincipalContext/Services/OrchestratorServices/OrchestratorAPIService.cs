using Application.PrincipalContext.Interfaces.Application;
using Application.PrincipalContext.Interfaces.Orchestrator;
using Domain.Nucleus.Entities.Application;
using Domain.Nucleus.Exceptions;
using Domain.Nucleus.Interfaces;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Linq;
using System.Threading.Tasks;
using Transversal.DTOs.Orchestrator.Login;

namespace Application.PrincipalContext.Services.OrchestratorServices
{
    public class OrchestratorAPIService : RestClient, IOrchestratorAPIService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogRequestAndResponseService _logRequestAndResponseService;
        private ResponseLoginDto _loginDto { get; set; }

        public OrchestratorAPIService(IUnitOfWork unitOfWork, ILogRequestAndResponseService logRequestAndResponseService, string url)
            : base(url)
        {
            this.Timeout = 120000;
            this.ReadWriteTimeout = 120000;

            this._unitOfWork = unitOfWork;
            this._logRequestAndResponseService = logRequestAndResponseService;
            Autenticate();
            if (_loginDto == null)
                throw new BusinessException("Invalid credentials");
        }

        private bool Autenticate()
        {
            if (_loginDto != null || _loginDto.Expiration > DateTime.Now)
                return true;

            var parameters = this._unitOfWork.ParametersRepository.GetAll();
            var UserOrchestrator = parameters.Where(x => x.Code == "UserOrchestrator").FirstOrDefault().Value;
            var PasswordOrchestrator = parameters.Where(x => x.Code == "PasswordOrchestrator").FirstOrDefault().Value;

            var userLoginDto = new RequestLoginDto
            {
                UserName = UserOrchestrator,
                Password = PasswordOrchestrator,
            };

            var request = new RestRequest("/Api/Login").AddJsonBody(userLoginDto);

            var logRequest = ProcessRequest(null, null, request).Result;

            var response = ExecutePostAsync(request).Result;

            ProcessResponse(logRequest, response);

            if (!response.IsSuccessful)
                Throw(response);

            this._loginDto = JsonConvert.DeserializeObject<ResponseLoginDto>(response.Content);
            return true;
        }

        public async Task<string> VehicleCounting(DateTime consultationDate)
        {
            var request = new RestRequest($"/api/ConteoVehiculos/{consultationDate}")
                .AddHeader("Authorization", _loginDto.Token);

            var logRequest = await ProcessRequest(null, consultationDate, request);

            var response = await ExecuteGetAsync(request);

            await ProcessResponse(logRequest, response);

            if (!response.IsSuccessful)
                Throw(response);

            return response.Content;

        }

        public async Task<string> VehicleCollection(DateTime consultationDate)
        {
            var request = new RestRequest($"/api/RecaudoVehiculos/{consultationDate}")
                .AddHeader("Authorization", _loginDto.Token);

            var logRequest = await ProcessRequest(null, consultationDate, request);

            var response = await ExecuteGetAsync(request);

            await ProcessResponse(logRequest, response);

            if (!response.IsSuccessful)
                Throw(response);

            return response.Content;
        }

        private async Task<LogRequestAndResponse> ProcessRequest(string Identifier, dynamic model, IRestRequest request)
        {
            try
            {
                var logRequestAndResponse = new LogRequestAndResponse();

                if (request != null)
                {
                    logRequestAndResponse.Identifier = Identifier;
                    logRequestAndResponse.Method = request.Resource;
                    logRequestAndResponse.Request = model == null ? null : JsonConvert.SerializeObject(model);
                    logRequestAndResponse.RequestDate = DateTime.Now;
                    logRequestAndResponse.Active = true;
                    logRequestAndResponse.IsDeleted = false;
                }

                logRequestAndResponse = await this._logRequestAndResponseService.InsertLogRequestAndResponse(logRequestAndResponse);

                return logRequestAndResponse;

            }
            catch (Exception Ex)
            {

                throw;
            }
        }

        private async Task<LogRequestAndResponse> ProcessResponse(LogRequestAndResponse logRequestAndResponse, IRestResponse response)
        {
            try
            {
                var logResponse = new
                {
                    Content = response.Content,
                };

                if (response != null)
                {
                    logRequestAndResponse.Response = JsonConvert.SerializeObject(logResponse);
                    logRequestAndResponse.ResponseDate = DateTime.Now;
                    logRequestAndResponse.ExceptionMessage = response.ErrorMessage;
                    logRequestAndResponse.ExceptionStackTrace = response.ErrorException == null ? null : JsonConvert.SerializeObject(response.ErrorException);
                }

                logRequestAndResponse = await this._logRequestAndResponseService.UpdateLogRequestAndResponse(logRequestAndResponse);

                return logRequestAndResponse;
            }
            catch
            {

                throw;
            }
        }

        private void Throw(IRestResponse response)
        {
            if (!string.IsNullOrEmpty(response.ErrorMessage))
            {
                throw new BusinessException(response.ErrorMessage);
            }

            throw new BusinessException(response.Content);
        }

        public void Dispose(){}

    }
}
