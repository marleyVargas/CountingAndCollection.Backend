using AutoMapper;
using Domain.Nucleus.Entities.Application;
using Domain.Nucleus.Entities.Transactional;
using Transversal.DTOs.Application;
using Transversal.DTOs.Transactional;
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
            CreateMap<ResponseVehicleCollectionDto, Collection>().ReverseMap();

            CreateMap<CollectionDto, Collection>()
                .ForMember(des => des.Amount, act => act.MapFrom(src => src.Cantidad))
                .ForMember(des => des.Category, act => act.MapFrom(src => src.Categoria))
                .ForMember(des => des.Direction, act => act.MapFrom(src => src.Sentido))
                .ForMember(des => des.Hour, act => act.MapFrom(src => src.Hora))
                .ForMember(des => des.QueryDate, act => act.MapFrom(src => src.Fecha))
                .ForMember(des => des.TabuledValue, act => act.MapFrom(src => src.ValorTabulado))
                .ForMember(des => des.Station, act => act.MapFrom(src => src.Estacion));

        }
    }
}
