using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiGateway.Interfaces
{
    public interface IHttpClientHelper
    {
        Task<ActionResult<TResult>> RestService<T, TResult>(string serviceUrl, HttpMethod Metodo, T content);
        Task<ActionResult<TResult>> RestService<TResult>(string serviceUrl, HttpMethod Metodo);
    }
}
