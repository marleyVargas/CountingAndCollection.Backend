using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : Controller, IDisposable
    {
        private IList<IDisposable> _disposables;

        public BaseController(params IDisposable[] disposables)
        {
            if (disposables != null)
            {
                _disposables = disposables;
            }
        }

        public new void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }

            _disposables = null;

        }

    }
}
