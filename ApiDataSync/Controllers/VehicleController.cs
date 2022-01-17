﻿using ApiDataSync.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Transversal.QueryFilters;
using Transversal.Response;

namespace ApiDataSync.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly ILogger<VehicleController> _logger;
        private readonly IHttpClientHelper _httpClientHelper;
        private IConfiguration _configuration;
        public string uriAPI;

        public VehicleController(ILogger<VehicleController> logger, IHttpClientHelper httpClientHelper, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientHelper = httpClientHelper;
            _configuration = configuration;
            uriAPI = _configuration.GetSection("APIServicesSettings:DistributedServices").Value;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<bool>>> Post(CollectionQueryFilter filter)
        {
            return await _httpClientHelper.RestService<CollectionQueryFilter, ApiResponse<bool>>($"{uriAPI}/api/Vehicle", HttpMethod.Post, filter);
        }
    }
}
