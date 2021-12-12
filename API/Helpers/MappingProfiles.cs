using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Entities.identity;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>()
            .ForMember(d => d.ProductBrand, s => s.MapFrom(s => s.ProductBrand.Name))
            .ForMember(d => d.ProductType, opt => opt.MapFrom(s => s.ProductType.Name))
            .ForMember(d => d.PictureUrl, opt => opt.MapFrom<ProductUrlResolver>());

            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}