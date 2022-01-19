using Domain.Nucleus.Entities.Transactional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transversal.QueryFilters;

namespace Application.PrincipalContext.Interfaces.Transactional
{
    public interface IVehicleService
    {
        Task<List<Collection>> GetVehicleCollectionByFilter(CollectionQueryFilter filters);
        Task<bool> SaveVehicleCounting(DateTime queryDate);
        Task<bool> SaveVehicleCollection(DateTime queryDate);
    }
}
