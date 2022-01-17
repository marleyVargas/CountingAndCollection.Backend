using Application.PrincipalContext.Interfaces.SchedulerManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transversal.DTOs.SchedulerManager;

namespace DistributedServices.SchedulerManagerServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoggerController : ControllerBase
    {
        private readonly ILogger<LoggerController> _logger;
        private readonly ILoggerService _loggerService;

        public LoggerController(ILogger<LoggerController> logger, ILoggerService loggerService)
        {
            this._logger = logger;
            this._loggerService = loggerService;
        }

        [HttpPost]
        public IActionResult WriteInLog(RequestWriteInLogDto requestDto)
        {
            this._logger.LogInformation("Entering the logger controller.");
            this._loggerService.WriteInLog(requestDto.Value);

            return Ok();
        }

    }
}
