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
using Transversal.DTOs.Application;
using Transversal.QueryFilters;
using Transversal.Response;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParameterController : ControllerBase
    {
        private readonly ILogger<ParameterController> _logger;
        private readonly IHttpClientHelper _httpClientHelper;
        private IConfiguration _configuration;
        public string uriAPI;

        public ParameterController(ILogger<ParameterController> logger, IHttpClientHelper httpClientHelper, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientHelper = httpClientHelper;
            _configuration = configuration;
            uriAPI = _configuration.GetSection("APIServicesSettings:DistributedServices").Value;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ParameterDto>>>> GetParameter([FromQuery] ParameterQueryFilter filters)
        {
            return await _httpClientHelper.RestService<ParameterQueryFilter, ApiResponse<IEnumerable<ParameterDto>>>($"{uriAPI}/api/Parameter", HttpMethod.Get, filters);

        }
    }
}
