using Domain.Nucleus.Entities.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Nucleus.Interfaces.Application
{
    public interface IParametersRepository : IRepository<Parameters>
    {
        Task<Parameters> GetParametersByCode(string code);
    }
}
