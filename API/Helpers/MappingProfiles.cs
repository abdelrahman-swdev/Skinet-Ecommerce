using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Entities.OrderAggregate;

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

            CreateMap<Core.Entities.identity.Address, AddressDto>().ReverseMap();

            CreateMap<Core.Entities.OrderAggregate.Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductItemId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.PictureUrl))
                .ForMember(d => d.PictureUrl, op => op.MapFrom<OrderItemUrlResolver>());
        }
    }
}