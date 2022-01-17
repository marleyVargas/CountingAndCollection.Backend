using ApiGateway.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Transversal.DTOs;
using Transversal.DTOs.Transactional.Request;
using Transversal.DTOs.Transactional.Response;
using Transversal.Response;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly IHttpClientHelper _httpClientHelper;
        private IConfiguration _configuration;
        public string uriAPI;
        public string uriPaymentPortal;

        public PaymentController(ILogger<TransactionController> logger, IHttpClientHelper httpClientHelper, IConfiguration configuration)
        {
            this._httpClientHelper = httpClientHelper;
            this._configuration = configuration;
            this._logger = logger;
            uriPaymentPortal = _configuration.GetSection("APIServicesSettings:UrlPaymentPortal").Value;
            uriAPI = _configuration.GetSection("APIServicesSettings:DistributedServices").Value;
        }

        [HttpPost]
        public async Task<ActionResult> Post()
        {
            if (Request == null)
                throw new ArgumentException("Bad Request");

            var showMode = Request.Form["show-mode"];
            var redirect = String.IsNullOrEmpty(Request.Form["redirect"]) || Convert.ToBoolean(Request.Form["redirect"]);

            try
            {

                List<ItemDto> items;
                try
                {
                    items = !string.IsNullOrEmpty(Request.Form["items"]) ?
                              JsonConvert.DeserializeObject<List<ItemDto>>(Request.Form["items"]) : null;
                }
                catch
                {
                    throw new ArgumentException("Invalid Items");
                }

                AddressDto address = new AddressDto();
                if (!string.IsNullOrEmpty(Request.Form["address"]))
                    address = new AddressDto
                    {
                        City = Request.Form["city"],
                        CountryCode = Request.Form["country"],
                        Neighborhood = Request.Form["neighborhood"],
                        PostalCode = Request.Form["postalCode"],
                        Direction = Request.Form["address"],
                    };

                var paymentData = new RequestPaymentDto
                {
                    AppKey = Request.Form["appKey"],
                    Concept = Request.Form["concept"],
                    Currency = Request.Form["currency"],
                    Method = Request.Form["method"],
                    Reference = Request.Form["reference"],
                    TotalValue = Convert.ToInt32(Request.Form["value"]),
                    Url = Request.Path, // ver url del que consume
                    CallbackUrl = Request.Form["callbackUrl"],
                    ReturnUrl = Request.Form["returnUrl"],
                    ShowMode = Request.Form["show-mode"],
                    MiniCart = new MiniCartDto
                    {
                        Buyer = new BuyerDto
                        {
                            DocumentNumber = Request.Form["document"],
                            DocumentTypeCode = Request.Form["documentType"],
                            Email = Request.Form["email"],
                            FirstName = Request.Form["firstName"],
                            LastName = Request.Form["lastName"],
                            Phone = Request.Form["phone"]
                        },
                        Address = !string.IsNullOrEmpty(Request.Form["address"]) ? address : null,
                        Items = items
                    }
                };

                if(!string.IsNullOrEmpty(Request.Form["ipAddress"]))
                    paymentData.IpAddress = Request.Form["ipAddress"];

                var resultTransaction = _httpClientHelper.RestService<RequestPaymentDto, ApiResponse<ResponsePaymentDto>>($"{uriAPI}/api/Payment", HttpMethod.Post, paymentData).Result.Result;
                try
                {
                    var result = (OkObjectResult)resultTransaction;
                    var tra = (ApiResponse<ResponsePaymentDto>)result.Value;                   
                    var url = $"{uriPaymentPortal}{ tra.data.GUID}?showMode={showMode}&isLpp3={ tra.data.IsReadyShortTerm }";
                    return redirect ? Redirect(url) : Ok(new
                    {
                        status = "OK",
                        urlRedirect = url,
                        description = "Payment Created"
                    });
                }
                catch
                {
                    BadRequestObjectResult error = (BadRequestObjectResult)resultTransaction;
                    if (error.StatusCode == 400)
                    {
                        var errors = (ErrorDto)error.Value;
                        throw new ArgumentException(errors.Errors.FirstOrDefault().Detail);
                    }
                }

                throw new ArgumentException("Internal Error");
            }
            catch(ArgumentException ex)
            {
                var url = $"{uriPaymentPortal}Error?showMode={showMode}&message={Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(ex.Message))}";
                return redirect ? Redirect(url) : BadRequest(new
                {
                    status = "ERROR",
                    urlRedirect = "",
                    description = ex.Message
                });
            }
            catch
            {
                var url = $"{uriPaymentPortal}Error?showMode={showMode}&message={Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Internal Error"))}";
                return redirect ? Redirect(url) : BadRequest(new
                {
                    status = "ERROR",
                    urlRedirect = "",
                    description = "Internal Error"
                });
            }
        }

    }
}