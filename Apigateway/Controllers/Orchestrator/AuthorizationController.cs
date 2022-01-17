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
using Transversal.DTOs.Orchestrator.Authorization;
using Transversal.Response;

namespace ApiGateway.Controllers.Orchestrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly ILogger<AuthorizationController> _logger;
        private readonly IHttpClientHelper _httpClientHelper;
        private IConfiguration _configuration;
        public string uriAPI;

        public AuthorizationController(ILogger<AuthorizationController> logger, IHttpClientHelper httpClientHelper, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientHelper = httpClientHelper;
            _configuration = configuration;
            uriAPI = _configuration.GetSection("APIServicesSettings:DistributedServices").Value;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ResponsePreApprovedDto>>> Post(RequestPreApprovedDto preApprovedDto)
        {
            return await _httpClientHelper.RestService<RequestPreApprovedDto, ApiResponse<ResponsePreApprovedDto>>($"{uriAPI}/api/Authorization", HttpMethod.Post, preApprovedDto);
        }
    }
}
