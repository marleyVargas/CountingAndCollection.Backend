using Application.PrincipalContext.Interfaces.Transactional;
using AutoMapper;
using Domain.Nucleus.CustomEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transversal.DTOs;
using Transversal.DTOs.Transactional;
using Transversal.DTOs.Transactional.Response;
using Transversal.QueryFilters;
using Transversal.Response;

namespace DistributedServices.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IMapper _mapper;

        public VehicleController(IVehicleService vehicleService, IMapper mapper)
        {
            this._vehicleService = vehicleService;
            this._mapper = mapper;
        }

        [HttpPost("GetCollection")]
        public IActionResult GetCollection(CollectionQueryFilter filters)
        {
            var result = this._vehicleService.GetVehicleCollectionByFilter(filters).Result;
            var collectionsDto = this._mapper.Map<List<ResponseVehicleCollectionDto>>(result).ToList();

            var metadata = new Metadata
            {
                TotalCount = result.TotalCount,
                PageSize = result.PageSize,
                CurrentPage = result.CurrentPage,
                TotalPages = result.TotalPages,
                HasNextPage = result.HasNextPage,
                HasPreviousPage = result.HasPreviousPage
            };

            var response = new ResponseCollectionPaginatorDto
            {
                data = collectionsDto,
                meta = metadata
            };

            return Ok(response);
        }

        [HttpPost("GetReportTabulatedValue")]
        public IActionResult GetReportTabulatedValue(CollectionQueryFilter filters)
        {
            var result = this._vehicleService.GetVehicleCollectionByDates(filters.CreatedDateInit.Value, filters.CreatedDateFin.Value).Result;

            var response = new ApiResponse<object>(result);
            return Ok(response);
        }

        [HttpGet("SaveCounting/{queryDate}")]
        public async Task<IActionResult> SaveCounting(DateTime queryDate)
        {
            var response = await this._vehicleService.SaveVehicleCounting(queryDate);
            return Ok(response);
        }

        [HttpGet("SaveCollection/{queryDate}")]
        public async Task<IActionResult> SaveCollection(DateTime queryDate)
        {
            var response = await this._vehicleService.SaveVehicleCollection(queryDate);
            return Ok(response);
        }


    }
}
