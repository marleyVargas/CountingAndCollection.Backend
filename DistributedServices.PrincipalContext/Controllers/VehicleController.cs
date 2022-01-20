using Application.PrincipalContext.Interfaces.Transactional;
using AutoMapper;
using Domain.Nucleus.CustomEntities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transversal.DTOs.Transactional;
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

            var listDates = result
                            .GroupBy(l => l.Station)
                            .Select(cl => new
                            {
                                Station = cl.Key,
                                DStation = cl.GroupBy(x => x.QueryDate)
                                        .Select(
                                            csLine => new
                                            {
                                                Date = csLine.Key,
                                                Quantity = csLine.Sum(c => c.Amount),
                                                Value = csLine.Sum(c => c.TabuledValue)
                                            }).ToList(),
                                Totals = cl.GroupBy(x => x.QueryDate)
                                        .Select(
                                            csLine => new
                                            {
                                                TotalsQuantity = cl.Sum(c => c.Amount),
                                                TotalsValue = cl.Sum(c => c.TabuledValue)
                                            }).FirstOrDefault(),
                            }).ToList();

            var totals = new
            {
                TotalsQuantity = listDates.Sum(x => x.Totals.TotalsQuantity),
                TotalsValue = listDates.Sum(x => x.Totals.TotalsValue)
            };

            var objResult = new
            {
                listDates,
                totals
            };

            var response = new ApiResponse<object>(objResult);
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
