using AutoMapper;
using MonShop.BackEnd.Common.Dto.Request;
using MonShop.BackEnd.DAL.Models;

namespace MonShop.BackEnd.DAL.Mapping;

public class MappingConfig
{
    public static MapperConfiguration RegisterMap()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<CartItemDto, CartItem>()
                .ForMember(desc => desc.ProductId, act => act.MapFrom(src => src.ProductId))
                .ForMember(desc => desc.Quantity, act => act.MapFrom(src => src.Quantity))
                .ForMember(desc => desc.SizeId, act => act.MapFrom(src => src.SizeId));

            config.CreateMap<ProductDto, Product>()
                .ForMember(desc => desc.ProductId, act => act.MapFrom(src => src.ProductId))
                .ForMember(desc => desc.ProductStatusId, act => act.MapFrom(src => src.ProductStatusId))
                .ForMember(desc => desc.ProductName, act => act.MapFrom(src => src.ProductName))
                .ForMember(desc => desc.CategoryId, act => act.MapFrom(src => src.CategoryId))
                .ForMember(desc => desc.Description, act => act.MapFrom(src => src.Description))
                .ForMember(desc => desc.Discount, act => act.MapFrom(src => src.Discount))
                ;
        });
        return mappingConfig;
    }
}