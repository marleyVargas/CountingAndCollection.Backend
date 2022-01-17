using ApiGateway.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Transversal.DTOs.Notifications;
using Transversal.DTOs.Orchestrator.Dummy;
using Transversal.Response;

namespace ApiGateway.Controllers.Orchestrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class DummyController : ControllerBase
    {
        private readonly ILogger<DummyController> _logger;
        private readonly IHttpClientHelper _httpClientHelper;
        private IConfiguration _configuration;
        public string uriAPI;

        public DummyController(ILogger<DummyController> logger, IHttpClientHelper httpClientHelper, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientHelper = httpClientHelper;
            _configuration = configuration;
            uriAPI = _configuration.GetSection("APIServicesSettings:DistributedServices").Value;
        }

        [HttpPost]
        [Route("ValidateFirm")]
        public async Task<ActionResult<bool>> ValidateFirm(RequestValidateFirmDto requestValidateFirmDto)
        {
            return await _httpClientHelper.RestService<RequestValidateFirmDto, bool>($"{uriAPI}/api/Dummy/ValidateFirm", HttpMethod.Post, requestValidateFirmDto);
        }

        [HttpPost]
        [Route("Correspondence")]
        public async Task<ActionResult<bool>> Correspondence(RequestCorrespondenceDto requestCorrespondenceDto)
        {
            return await _httpClientHelper.RestService<RequestCorrespondenceDto, bool>($"{uriAPI}/api/Dummy/Correspondence", HttpMethod.Post, requestCorrespondenceDto);
        }

        [HttpPost]
        [Route("Callback")]
        public async Task<ActionResult<ApiResponse<ResponsePaymentConfirmationDto>>> Callback(ResponsePaymentConfirmationDto responsePaymentConfirmationDto)
        {
            return await _httpClientHelper.RestService<ResponsePaymentConfirmationDto, ApiResponse<ResponsePaymentConfirmationDto>>($"{uriAPI}/api/Dummy/Callback", HttpMethod.Post, responsePaymentConfirmationDto);
        }

    }
}
