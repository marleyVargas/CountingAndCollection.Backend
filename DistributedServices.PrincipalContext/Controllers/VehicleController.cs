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
using System.Runtime.Intrinsics.X86;
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
    public class VehicleController : ApiControllerBase
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
            return HandleResult(result.Result, result.ErrorProvider);
        }

        [HttpPost("GetReportTabulatedValue")]
        public IActionResult GetReportTabulatedValue(CollectionQueryFilter filters)
        {
            var result = this._vehicleService.GetVehicleCollectionByDates(filters.CreatedDateInit.Value, filters.CreatedDateFin.Value).Result;
            return HandleResult(result.Result, result.ErrorProvider);
        }

        [HttpGet("SaveCounting/{queryDate}")]
        public async Task<IActionResult> SaveCounting(DateTime queryDate)
        {
            var result = await this._vehicleService.SaveVehicleCounting(queryDate);
            return HandleResult(result.Result, result.ErrorProvider);
        }

        [HttpGet("SaveCollection/{queryDate}")]
        public async Task<IActionResult> SaveCollection(DateTime queryDate)
        {
            var result = await this._vehicleService.SaveVehicleCollection(queryDate);
            return HandleResult(result.Result, result.ErrorProvider);
        }


    }
}
