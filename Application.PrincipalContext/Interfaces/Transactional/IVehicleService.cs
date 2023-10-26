using Domain.Nucleus.CustomEntities;
using Domain.Nucleus.Entities.Transactional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transversal.DTOs.Transactional.Response;
using Transversal.DTOs;
using Transversal.QueryFilters;

namespace Application.PrincipalContext.Interfaces.Transactional
{
    public interface IVehicleService
    {
        Task<Response<ResponseCollectionPaginatorDto>> GetVehicleCollectionByFilter(CollectionQueryFilter filters);

        Task<Response<object>> GetVehicleCollectionByDates(DateTime dateInit, DateTime dateEnd);

        Task<Response<bool>> SaveVehicleCounting(DateTime queryDate);
        Task<Response<bool>> SaveVehicleCollection(DateTime queryDate);
    }
}
