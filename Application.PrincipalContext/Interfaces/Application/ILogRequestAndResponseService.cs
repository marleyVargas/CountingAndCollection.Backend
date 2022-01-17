using Domain.Nucleus.Entities.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PrincipalContext.Interfaces.Application
{
    public interface ILogRequestAndResponseService
    {
        Task<LogRequestAndResponse> InsertLogRequestAndResponse(LogRequestAndResponse logRequestAndResponse);

        Task<LogRequestAndResponse> UpdateLogRequestAndResponse(LogRequestAndResponse logRequestAndResponse);
    }
}
