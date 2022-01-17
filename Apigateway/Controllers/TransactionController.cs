using ApiGateway.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Transversal.DTOs.Application;
using Transversal.DTOs.Transactional.Request;
using Transversal.DTOs.Transactional.Response;
using Transversal.QueryFilters;
using Transversal.Response;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly IHttpClientHelper _httpClientHelper;
        private IConfiguration _configuration;
        public string uriAPI;

        public TransactionController(ILogger<TransactionController> logger, IHttpClientHelper httpClientHelper, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientHelper = httpClientHelper;
            _configuration = configuration;
            uriAPI = _configuration.GetSection("APIServicesSettings:DistributedServices").Value;
        }


        [HttpGet]
        public async Task<ActionResult<ApiResponse<ResponseVehicleCollectionDto>>> GetReportCollections(CollectionQueryFilter guid)
        {
            return await _httpClientHelper.RestService<ApiResponse<ResponseVehicleCollectionDto>>($"{uriAPI}/api/Transaction/{guid}", HttpMethod.Get);
        }

        

    }
}
