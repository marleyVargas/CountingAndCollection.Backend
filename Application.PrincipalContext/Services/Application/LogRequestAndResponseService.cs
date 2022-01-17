using Application.PrincipalContext.Interfaces.Application;
using Domain.Nucleus.CustomEntities;
using Domain.Nucleus.Entities.Application;
using Domain.Nucleus.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PrincipalContext.Services.Application
{
    public class LogRequestAndResponseService : ILogRequestAndResponseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;

        public LogRequestAndResponseService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> options)
        {
            this._unitOfWork = unitOfWork;
            this._paginationOptions = options.Value;
        }

        public async Task<LogRequestAndResponse> InsertLogRequestAndResponse(LogRequestAndResponse logRequestAndResponse)
        {
            await this._unitOfWork.LogRequestAndResponseRepository.Add(logRequestAndResponse);
            await this._unitOfWork.SaveChangesAsync();

            return logRequestAndResponse;
        }

        public async Task<LogRequestAndResponse> UpdateLogRequestAndResponse(LogRequestAndResponse logRequestAndResponse)
        {
            this._unitOfWork.LogRequestAndResponseRepository.Update(logRequestAndResponse);
            await this._unitOfWork.SaveChangesAsync();

            return logRequestAndResponse;
        }
    }
}
