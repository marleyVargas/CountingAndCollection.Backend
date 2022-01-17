using ApiGateway.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Transversal.DTOs.Application;
using Transversal.DTOs.Application.Response;
using Transversal.Response;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferenciaTransactionController : ControllerBase
    {

        private readonly ILogger<ReferenciaTransactionController> _logger;
        private readonly IHttpClientHelper _httpClientHelper;
        private IConfiguration _configuration;
        public string uriAPI;

        public ReferenciaTransactionController(ILogger<ReferenciaTransactionController> logger, IHttpClientHelper httpClientHelper, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientHelper = httpClientHelper;
            _configuration = configuration;
            uriAPI = _configuration.GetSection("APIServicesSettings:DistributedServices").Value;
        }

        //[HttpGet]
        //public async Task<ActionResult<ApiResponse<IEnumerable<DocumentTypeDto>>>> GetDocumentTypes([FromQuery] DocumentTypeQueryFilter filters)
        //{
        //    return await _httpClientHelper.RestService<DocumentTypeQueryFilter, ApiResponse<IEnumerable<DocumentTypeDto>>>($"{uriAPI}/api/DocumentType", HttpMethod.Get, filters);

        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult<ApiResponse<DocumentTypeDto>>> GetDocumentType(long id)
        //{
        //    return await _httpClientHelper.RestService<ApiResponse<DocumentTypeDto>>($"{uriAPI}/api/DocumentType/{id}", HttpMethod.Get);
        //}

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ResponseReferenciaTransactionDto>>> Post(RequestReferenciaTransactionDto referenciaTransactionDto)
        {
            return await _httpClientHelper.RestService<RequestReferenciaTransactionDto, ApiResponse<ResponseReferenciaTransactionDto>>($"{uriAPI}/api/ReferenciaTransaction", HttpMethod.Post, referenciaTransactionDto);
        }

        //[HttpPut("{id}")]
        //public async Task<ActionResult<bool>> Put(long id, DocumentTypeDto documentTypeDto)
        //{
        //    return await _httpClientHelper.RestService<DocumentTypeDto, bool>($"{uriAPI}/api/DocumentType/{id}", HttpMethod.Put, documentTypeDto);
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult<bool>> Delete(long id)
        //{
        //    return await _httpClientHelper.RestService<bool>($"{uriAPI}/api/DocumentType/{id}", HttpMethod.Delete);
        //}

    }
}
