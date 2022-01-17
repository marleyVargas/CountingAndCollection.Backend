﻿using ApiGateway.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Transversal.DTOs.Orchestrator.Correspondence;
using Transversal.Response;

namespace ApiGateway.Controllers.Orchestrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorrespondenceController : ControllerBase
    {
        private readonly ILogger<CorrespondenceController> _logger;
        private readonly IHttpClientHelper _httpClientHelper;
        private IConfiguration _configuration;
        public string uriAPI;

        public CorrespondenceController(ILogger<CorrespondenceController> logger, IHttpClientHelper httpClientHelper, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientHelper = httpClientHelper;
            _configuration = configuration;
            uriAPI = _configuration.GetSection("APIServicesSettings:DistributedServices").Value;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ResponseTransactionCorrespondenceDto>>> TransactionCorrespondence(RequestTransactionCorrespondenceDto requestDto)
        {
            return await _httpClientHelper.RestService<RequestTransactionCorrespondenceDto, ApiResponse<ResponseTransactionCorrespondenceDto>>($"{uriAPI}/api/Correspondence", HttpMethod.Post, requestDto);
        }
    }
}
