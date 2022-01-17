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
using Transversal.DTOs.Orchestrator.Tin;
using Transversal.Response;

namespace ApiGateway.Controllers.Orchestrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class TinController : ControllerBase
    {
        private readonly ILogger<TinController> _logger;
        private readonly IHttpClientHelper _httpClientHelper;
        private IConfiguration _configuration;
        public string uriAPI;

        public TinController(ILogger<TinController> logger, IHttpClientHelper httpClientHelper, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientHelper = httpClientHelper;
            _configuration = configuration;
            uriAPI = _configuration.GetSection("APIServicesSettings:DistributedServices").Value;
        }

        [HttpPost]
        [Route("GenerateTin")]
        public async Task<ActionResult<ApiResponse<ResponseGenerateTinDto>>> GenerateTin(RequestGenerateTinDto requetsGenerateTinDto)
        {
            return await _httpClientHelper.RestService<RequestGenerateTinDto, ApiResponse<ResponseGenerateTinDto>>($"{uriAPI}/api/Tin/GenerateTin", HttpMethod.Post, requetsGenerateTinDto);
        }

        [HttpPost]
        [Route("ValidateTin")]
        public async Task<ActionResult<ApiResponse<ResponseValidateTinDto>>> ValidateTin(RequestValidateTinDto requetsValidateTinDto)
        {
            return await _httpClientHelper.RestService<RequestValidateTinDto, ApiResponse<ResponseValidateTinDto>>($"{uriAPI}/api/Tin/ValidateTin", HttpMethod.Post, requetsValidateTinDto);
        }

    }
}
