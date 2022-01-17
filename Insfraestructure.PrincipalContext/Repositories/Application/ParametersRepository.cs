using Domain.Nucleus.Entities.Application;
using Domain.Nucleus.Interfaces.Application;
using Insfraestructure.PrincipalContext.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insfraestructure.PrincipalContext.Repositories.Application
{
    public class ParametersRepository : BaseRepository<Parameters>, IParametersRepository
    {
        public ParametersRepository(CountingAndCollectionContext context) : base(context)
        {

        }

        public async Task<Parameters> GetParametersByCode(string code)
        {
            return await this._entities.Where(x => x.Code.Trim().ToLower() == code.Trim().ToLower()).FirstOrDefaultAsync();
        }
    }
}
