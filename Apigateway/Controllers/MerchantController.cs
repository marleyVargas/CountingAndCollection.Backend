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
using Transversal.DTOs.Application.Request;
using Transversal.QueryFilters;
using Transversal.Response;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantController : ControllerBase
    {
        private readonly ILogger<MerchantController> _logger;
        private readonly IHttpClientHelper _httpClientHelper;
        private IConfiguration _configuration;
        public string uriAPI;

        public MerchantController(ILogger<MerchantController> logger, IHttpClientHelper httpClientHelper, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientHelper = httpClientHelper;
            _configuration = configuration;
            uriAPI = _configuration.GetSection("APIServicesSettings:DistributedServices").Value;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<MerchantDto>>>> GetDocumentTypes([FromQuery] MerchantQueryFilter filters)
        {
            return await _httpClientHelper.RestService<MerchantQueryFilter, ApiResponse<IEnumerable<MerchantDto>>>($"{uriAPI}/api/Merchant", HttpMethod.Get, filters);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<MerchantDto>>> GetMerchant(long id)
        {
            return await _httpClientHelper.RestService<ApiResponse<MerchantDto>>($"{uriAPI}/api/Merchant/{id}", HttpMethod.Get);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<IEnumerable<bool>>>> Post([FromQuery] MerchantRequestDto merchantRequestDto)
        {
            return await _httpClientHelper.RestService<MerchantRequestDto, ApiResponse<IEnumerable<bool>>>($"{uriAPI}/api/Merchant", HttpMethod.Post, merchantRequestDto);
        }

    }
}
