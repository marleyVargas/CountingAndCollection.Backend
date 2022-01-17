using Application.PrincipalContext.Interfaces.Application;
using Application.PrincipalContext.Interfaces.Transactional;
using AutoMapper;
using Domain.Nucleus.CustomEntities;
using Domain.Nucleus.Entities.Application;
using Domain.Nucleus.Entities.Transactional;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transversal.DTOs.Application;
using Transversal.DTOs.Transactional.Request;
using Transversal.DTOs.Transactional.Response;
using Transversal.QueryFilters;
using Transversal.Response;

namespace DistributedServices.PrincipalContext.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IMapper _mapper;

        public VehicleController(IVehicleService vehicleService, IMapper mapper)
        {
            this._vehicleService = vehicleService;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetReportTabulatedValue(CollectionQueryFilter filters)
        {
            var result = this._vehicleService.GetVehicleCollectionByFilter(filters).Result;

            var report = result.GroupBy(x => x.Station, x => x.CreatedDate)
                .Select(grp => grp.ToList())
                .ToList();

            var metadata = new Metadata
            {
                //TotalCount = response.TotalCount,
                //PageSize = response.PageSize,
                //CurrentPage = response.CurrentPage,
                //TotalPages = response.TotalPages,
                //HasNextPage = response.HasNextPage,
                //HasPreviousPage = response.HasPreviousPage
            };

            var response = new ApiResponse<IEnumerable<object>>(report)
            {
                Meta = metadata
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SaveCounting(DateTime queryDate)
        {
            var response = await this._vehicleService.SaveVehicleCounting(queryDate);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SaveCollection(DateTime queryDate)
        {
            var response = await this._vehicleService.SaveVehicleCollection(queryDate);

            return Ok(response);
        }

      
    }
}
