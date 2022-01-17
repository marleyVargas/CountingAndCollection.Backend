using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using ApiGateway.Interfaces;
using Transversal.DTOs.Application;
using Transversal.DTOs.Transactional.Request;
using Transversal.DTOs.Transactional.Response;
using Transversal.DTOs.Notifications;
using Transversal.QueryFilters;
using Transversal.Response;
using System.Net.Http;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly IHttpClientHelper _httpClientHelper;
        private IConfiguration _configuration;
        public string uriAPI;


        public NotificationController(ILogger<TransactionController> logger, IHttpClientHelper httpClientHelper, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientHelper = httpClientHelper;
            _configuration = configuration;
            uriAPI = _configuration.GetSection("APIServicesSettings:DistributedServices").Value;
        }

        [HttpPost]
        [Route("Notification")]
        public async Task<ActionResult<bool>> SendPaymentNotification(RequestSendNotificationDto requestSendNotification)
        {
            return await _httpClientHelper.RestService<RequestSendNotificationDto, bool>($"{uriAPI}/api/Notification/SendPaymentNotification", HttpMethod.Post, requestSendNotification);
        }
    }
}
