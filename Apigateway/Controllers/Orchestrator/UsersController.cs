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
using Transversal.DTOs.Orchestrator.Users;
using Transversal.Response;

namespace ApiGateway.Controllers.Orchestrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IHttpClientHelper _httpClientHelper;
        private IConfiguration _configuration;
        public string uriAPI;

        public UsersController(ILogger<UsersController> logger, IHttpClientHelper httpClientHelper, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientHelper = httpClientHelper;
            _configuration = configuration;
            uriAPI = _configuration.GetSection("APIServicesSettings:DistributedServices").Value;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<ResponseLoginDto>>> Authenticate()
        {
            return await _httpClientHelper.RestService<ApiResponse<ResponseLoginDto>>($"{uriAPI}/api/Users", HttpMethod.Get);
        }
    }
}
