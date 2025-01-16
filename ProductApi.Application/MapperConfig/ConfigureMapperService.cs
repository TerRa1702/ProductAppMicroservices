using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.Application.DTOs;
using ProductApi.Domain.Entities;
using SharedLibrary.Interfaces;
using SharedLibrary.Mapper;

namespace ProductApi.Application.MapperConfig
{
    public static class ConfigureMapperService
    {
        public static IServiceCollection ConfigureMapper(this IServiceCollection services)
        {
            services.AddSingleton<IMapper>(_ => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDTO>();
                cfg.CreateMap<ProductDTO, Product>();
            }).CreateMapper());

            services.AddSingleton<IBaseMapper<Product, ProductDTO>, BaseMapper<Product, ProductDTO>>();
            services.AddSingleton<IBaseMapper<ProductDTO, Product>, BaseMapper<ProductDTO, Product>>();

            return services;
        }
    }
}
