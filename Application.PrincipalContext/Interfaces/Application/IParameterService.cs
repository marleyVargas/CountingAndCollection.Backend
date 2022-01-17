using Domain.Nucleus.Entities.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transversal.QueryFilters;

namespace Application.PrincipalContext.Interfaces.Application
{
    public interface IParameterService
    {
        IEnumerable<Parameters> GetParameterByFilter(ParameterQueryFilter filters);
    }
}
