using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using SharedLibrary.Interfaces;
using SharedLibrary.Responses;

namespace ProductApi.Endpoints
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("api/Product");

            group.MapGet("/", async (IProduct productInterface,
                IBaseMapper<Product, ProductDTO> mapper,
                CancellationToken cancellationToken) =>
            {
                var entities = await productInterface.GetAllAsync(cancellationToken);

                if (!entities.Any())
                    return Results.NotFound("No products detected in database");

                var products = mapper.MapList(entities);

                return products.Any() ? Results.Ok(products) : Results.NotFound("No products found");
            })
                .WithName("GetAllProducts")
                .Produces<IEnumerable<ProductDTO>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            group.MapGet("/{id:int}", async (IProduct productInterface,
                IBaseMapper<Product, ProductDTO> mapper,
                int id,
                CancellationToken cancellationToken) =>
            {
                var entity = await productInterface.FindByIdAsync(id, cancellationToken);

                if (entity is null)
                    return Results.NotFound("Requested product not found");

                var product = mapper.MapDTO(entity);
                return Results.Ok(product);
            })
                .WithName("GetProductById")
                .Produces<ProductDTO>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            group.MapPost("/", async (IProduct productInterface,
                IBaseMapper<ProductDTO, Product> mapper,
                [FromBody] ProductDTO product,
                CancellationToken cancellationToken) =>
            {
                var entity = mapper.MapDTO(product);
                var response = await productInterface.CreateAsync(entity, cancellationToken);
                return response.Flag ? Results.Ok(response) : Results.BadRequest(response);
            })
                .WithName("CreateProduct")
                .Produces<Response>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            group.MapPut("/", async (IProduct productInterface,
                IBaseMapper<ProductDTO, Product> mapper,
                [FromBody] ProductDTO product,
                CancellationToken cancellationToken) =>
            {
                var entity = mapper.MapDTO(product);
                var response = await productInterface.CreateAsync(entity, cancellationToken);
                return response.Flag ? Results.Ok(response) : Results.BadRequest(response);
            })
                .WithName("UpdateProduct")
                .Produces<Response>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            group.MapDelete("/", async (IProduct productInterface,
                [FromBody] ProductDTO product,
                CancellationToken cancellationToken) =>
            {
                var entity = await productInterface.FindByIdAsync(product.Id, cancellationToken);
                var response = await productInterface.DeleteAsync(entity, cancellationToken);
                return response.Flag ? Results.Ok(response) : Results.BadRequest(response);
            })
                .WithName("DeleteProduct")
                .Produces<Response>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
