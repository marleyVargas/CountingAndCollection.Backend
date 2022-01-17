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
using Transversal.DTOs.Orchestrator.OTPSign;
using Transversal.Response;

namespace ApiGateway.Controllers.Orchestrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class OTPSignController : ControllerBase
    {
        private readonly ILogger<OTPSignController> _logger;
        private readonly IHttpClientHelper _httpClientHelper;
        private IConfiguration _configuration;
        public string uriAPI;

        public OTPSignController(ILogger<OTPSignController> logger, IHttpClientHelper httpClientHelper, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientHelper = httpClientHelper;
            _configuration = configuration;
            uriAPI = _configuration.GetSection("APIServicesSettings:DistributedServices").Value;
        }

        [HttpPost]
        [Route("GenerateOTPPromissoryNote")]
        public async Task<ActionResult<ApiResponse<ResponseGenerateOTPPromissoryNoteDto>>> GenerateOTPPromissoryNote(RequestGenerateOTPPromissoryNoteDto requestDto)
        {
            return await _httpClientHelper.RestService<RequestGenerateOTPPromissoryNoteDto, ApiResponse<ResponseGenerateOTPPromissoryNoteDto>>($"{uriAPI}/api/OTPSign/GenerateOTPPromissoryNote", HttpMethod.Post, requestDto);
        }

        [HttpPost]
        [Route("ValidateOTPPromissoryNote")]
        public async Task<ActionResult<ApiResponse<ResponseValidateOTPPromissoryNoteDto>>> ValidateOTPPromissoryNote(RequestValidateOTPPromissoryNoteDto requestDto)
        {
            return await _httpClientHelper.RestService<RequestValidateOTPPromissoryNoteDto, ApiResponse<ResponseValidateOTPPromissoryNoteDto>>($"{uriAPI}/api/OTPSign/ValidateOTPPromissoryNote", HttpMethod.Post, requestDto);
        }
    }
}
