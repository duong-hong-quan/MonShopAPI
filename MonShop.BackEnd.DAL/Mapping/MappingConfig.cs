using AutoMapper;
using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.BackEnd.DAL.Mapping
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMap()
        {

            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<DeliveryAddressDTO, DeliveryAddress>().
                ForMember(desc => desc.DeliveryAddressId, act => act.MapFrom(src => src.DeliveryAddressId))
                .ForMember(desc => desc.ApplicationUserId, act => act.MapFrom(src => src.ApplicationUserId))
                .ForMember(desc => desc.Address, act => act.MapFrom(src => src.Address));

                config.CreateMap<CategoryDTO, Category>().
              ForMember(desc => desc.CategoryId, act => act.MapFrom(src => src.CategoryId))
              .ForMember(desc => desc.CategoryName, act => act.MapFrom(src => src.CategoryName));
            });
            return mappingConfig;
        }

    }
}
