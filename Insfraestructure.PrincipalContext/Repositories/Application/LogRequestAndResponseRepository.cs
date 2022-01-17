using Domain.Nucleus.Entities.Application;
using Domain.Nucleus.Interfaces.Application;
using Insfraestructure.PrincipalContext.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insfraestructure.PrincipalContext.Repositories.Application
{
    public class LogRequestAndResponseRepository : BaseRepository<LogRequestAndResponse>, ILogRequestAndResponseRepository
    {
        public LogRequestAndResponseRepository(CountingAndCollectionContext context) : base(context)
        {

        }
    }
}
