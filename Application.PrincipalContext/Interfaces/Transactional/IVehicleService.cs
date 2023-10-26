using Domain.Nucleus.CustomEntities;
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
        Task<PagedList<Collection>> GetVehicleCollectionByFilter(CollectionQueryFilter filters);

        Task<object> GetVehicleCollectionByDates(DateTime dateInit, DateTime dateEnd);

        Task<bool> SaveVehicleCounting(DateTime queryDate);
        Task<bool> SaveVehicleCollection(DateTime queryDate);
    }
}
