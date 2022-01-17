using Application.PrincipalContext.Interfaces.Application;
using Domain.Nucleus.Entities.Application;
using Domain.Nucleus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Transversal.QueryFilters;

namespace Application.PrincipalContext.Services.Application
{
    public class ParameterService : IParameterService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ParameterService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<Parameters> GetParameterByFilter(ParameterQueryFilter filters)
        {
            Expression<Func<Parameters, bool>> query = q =>
            (
               q.Code != null ? q.Code == filters.Code : q.Code == q.Code
            );

            var parameters = this._unitOfWork.ParametersRepository.GetByFilter(query).ToList();

            return parameters;
        }
    }
}
