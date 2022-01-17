using Domain.Nucleus.Entities.Transactional;
using Domain.Nucleus.Interfaces.Transactional;
using Insfraestructure.PrincipalContext.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Insfraestructure.PrincipalContext.Repositories.Transactional
{
    public class CollectionRepository : BaseRepository<Collection>, ICollectionRepository
    {
        public CollectionRepository(CountingAndCollectionContext context) : base(context)
        {

        }
     
    }
}
