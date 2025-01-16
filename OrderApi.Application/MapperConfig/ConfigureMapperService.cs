using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.DTOs;
using OrderApi.Domain.Entites;
using SharedLibrary.Interfaces;
using SharedLibrary.Mapper;

namespace OrderApi.Application.MapperConfig
{
    public static class ConfigureMapperService
    {
        public static IServiceCollection ConfigureMapper(this IServiceCollection services)
        {
            services.AddSingleton<IMapper>(_ => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderDTO>();
                cfg.CreateMap<OrderDTO, Order>();
            }).CreateMapper());

            services.AddSingleton<IBaseMapper<Order, OrderDTO>, BaseMapper<Order, OrderDTO>>();
            services.AddSingleton<IBaseMapper<OrderDTO, Order>, BaseMapper<OrderDTO, Order>>();

            return services;
        }
    }
}
