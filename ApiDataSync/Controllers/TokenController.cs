using ApiDataSync.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Transversal.DTOs.Account;

namespace ApiDataSync.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IHttpClientHelper _httpClientHelper;
        private IConfiguration _configuration;
        public string uriAPI;

        public TokenController(IHttpClientHelper httpClientHelper, IConfiguration configuration)
        {
            _httpClientHelper = httpClientHelper;
            _configuration = configuration;
            uriAPI = _configuration.GetSection("APIServicesSettings:DistributedServices").Value;
        }

        [HttpPost]
        [Route("Authentication")]
        public async Task<ActionResult<ResponseSecurityDto>> Authentication(RequestSecurityDto requestSecurityDto)
        {
            return await _httpClientHelper.RestService<RequestSecurityDto, ResponseSecurityDto>($"{uriAPI}/api/Token/Authentication", HttpMethod.Post, requestSecurityDto);
        }
    }
}
