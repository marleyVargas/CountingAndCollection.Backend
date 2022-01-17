using ApiGateway.Interfaces;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Transversal.DTOs;

namespace ApiGateway.Helpers
{
    public class HttpClientHelper : IHttpClientHelper
    {
        HttpContext _context;
        IHttpClientFactory _clientFactory;

        public HttpClientHelper(IHttpContextAccessor httpContextAccessor, IHttpClientFactory clientFactory)
        {
            _context = httpContextAccessor.HttpContext;
            _clientFactory = clientFactory;
        }

        private HttpContent CreateHttpContent<T>(T content)
        {
            string json = JsonConvert.SerializeObject(content);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public async Task<ActionResult<TResult>> RestService<T, TResult>(string serviceUrl, HttpMethod Metodo, T content)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            string uri = serviceUrl;
            string token = await _context.GetTokenAsync("access_token");

            using (var client = _clientFactory.CreateClient("HttpClientWithSSLUntrusted"))
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                var request = new HttpRequestMessage(Metodo, uri);

                if (content != null)
                {
                    HttpContent httpContent = CreateHttpContent(content);
                    request.Content = httpContent;
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }

                if (!string.IsNullOrEmpty(token))
                {
                    client.SetBearerToken(token);
                }

                HttpResponseMessage asyncRes = await client.SendAsync(request);

                if (asyncRes.Headers.Contains("X-Pagination"))
                {
                    _context.Response.Headers.Add("X-Pagination", asyncRes.Headers.GetValues("X-Pagination").FirstOrDefault() ?? "");
                }

                if (asyncRes.Headers.Contains("x-total-notificaciones"))
                {
                    _context.Response.Headers.Add("x-total-notificaciones", asyncRes.Headers.GetValues("x-total-notificaciones").FirstOrDefault() ?? "");
                }

                if (asyncRes.Headers.Contains("x-current-page"))
                {
                    _context.Response.Headers.Add("x-current-page", asyncRes.Headers.GetValues("x-current-page").FirstOrDefault() ?? "");
                    _context.Response.Headers.Add("x-items-per-page", asyncRes.Headers.GetValues("x-items-per-page").FirstOrDefault() ?? "");
                    _context.Response.Headers.Add("x-total-items", asyncRes.Headers.GetValues("x-total-items").FirstOrDefault() ?? "");
                    _context.Response.Headers.Add("x-total-pages", asyncRes.Headers.GetValues("x-total-pages").FirstOrDefault() ?? "");
                    _context.Response.Headers.Add("Access-Control-Expose-Headers", new string[] { "x-current-page", "x-items-per-page", "x-total-items", "x-total-pages" });
                }

                var res = await asyncRes.Content.ReadAsStringAsync();

                if (asyncRes.StatusCode == HttpStatusCode.OK)
                {
                    TResult resul = JsonConvert.DeserializeObject<TResult>(res);
                    return new OkObjectResult(resul);

                }
                else if (asyncRes.StatusCode == HttpStatusCode.NotFound)
                {
                    return new NotFoundObjectResult("Not Found");
                }
                else if (asyncRes.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return new UnauthorizedObjectResult("Not Authorized");
                }
                else if (asyncRes.StatusCode == HttpStatusCode.Forbidden)
                {
                    return new ForbidResult("Not Authorized for this resource");
                }
                else
                {
                    ErrorDto error = JsonConvert.DeserializeObject<ErrorDto>(res);
                    return new BadRequestObjectResult(error);
                }
            }
        }

        public async Task<ActionResult<TResult>> RestService<TResult>(string serviceUrl, HttpMethod Metodo)
        {
            return await RestService<object, TResult>(serviceUrl, Metodo, null);
        }
    }
}
