using AutoMapper;
using ECommerce.Api.Dtos;
using ECommerce.Api.Entities;

namespace ECommerce.Api.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Customers, CustomerDto>();
        CreateMap<Orders, OrderDto>()
            .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate.ToString("dd-MMM-yyyy")))
            .ForMember(dest => dest.DeliveryExpected, opt => opt.MapFrom(src => src.DeliveryExpected.ToString("dd-MMM-yyyy")))
            .ForMember(dest => dest.DeliveryAddress, opt => opt.Ignore());
        CreateMap<OrderItems, OrderItemDto>()
            .ForMember(dest => dest.PriceEach, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Order.ContainsGift ? "Gift" : src.Product.ProductName));
    }
}
