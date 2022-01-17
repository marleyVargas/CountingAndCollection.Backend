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
using Transversal.DTOs.Orchestrator.Reverse;
using Transversal.Response;

namespace ApiGateway.Controllers.Orchestrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReverseController : ControllerBase
    {
        private readonly ILogger<ReverseController> _logger;
        private readonly IHttpClientHelper _httpClientHelper;
        private IConfiguration _configuration;
        public string uriAPI;

        public ReverseController(ILogger<ReverseController> logger, IHttpClientHelper httpClientHelper, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientHelper = httpClientHelper;
            _configuration = configuration;
            uriAPI = _configuration.GetSection("APIServicesSettings:DistributedServices").Value;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ResponseReverseAuthorizationDto>>> Post(RequestReverseAuthorizationDto requestReverseDto)
        {
            return await _httpClientHelper.RestService<RequestReverseAuthorizationDto, ApiResponse<ResponseReverseAuthorizationDto>>($"{uriAPI}/api/Reverse", HttpMethod.Post, requestReverseDto);
        }
    }
}
