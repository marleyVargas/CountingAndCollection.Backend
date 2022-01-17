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
using Transversal.DTOs.Application.Request;
using Transversal.DTOs.Application.Response;
using Transversal.Response;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly IHttpClientHelper _httpClientHelper;
        private IConfiguration _configuration;
        public string uriAPI;

        public CustomerController(ILogger<CustomerController> logger, IHttpClientHelper httpClientHelper, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientHelper = httpClientHelper;
            _configuration = configuration;
            uriAPI = _configuration.GetSection("APIServicesSettings:DistributedServices").Value;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ResponseCustomerDto>>> Post(RequestCustomerDto requestCustomerDto)
        {
            return await _httpClientHelper.RestService<RequestCustomerDto, ApiResponse<ResponseCustomerDto>>($"{uriAPI}/api/Customer", HttpMethod.Post, requestCustomerDto);
        }

        [HttpPost("Update")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateCustomer(RequestCustomerDto requestCustomerDto)
        {
            return await _httpClientHelper.RestService<RequestCustomerDto, ApiResponse<bool>>($"{uriAPI}/api/Customer/Update", HttpMethod.Post, requestCustomerDto);
        }

    }
}
