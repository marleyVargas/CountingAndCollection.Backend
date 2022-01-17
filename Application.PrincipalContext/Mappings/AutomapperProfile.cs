using AutoMapper;
using Domain.Nucleus.Entities.Application;
using Domain.Nucleus.Entities.Transactional;
using Transversal.DTOs.Application;

using Transversal.DTOs.Transactional.Request;
using Transversal.DTOs.Transactional.Response;

namespace Application.PrincipalContext.Mappings
{
    class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Parameters, ParameterDto>().ReverseMap();
            CreateMap<Collection, ResponseVehicleCollectionDto>().ReverseMap();
        }
    }
}
