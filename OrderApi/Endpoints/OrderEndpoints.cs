using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrderApi.Application.DTOs;
using OrderApi.Application.Interfaces;
using OrderApi.Application.Services;
using OrderApi.Domain.Entites;
using SharedLibrary.Interfaces;
using SharedLibrary.Responses;

namespace OrderApi.Endpoints
{
    public static class OrderEndpoints
    {
        public static void MapOrderEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("api/Order");

            group.MapGet("/", async (IOrder orderInterface,
                IBaseMapper<Order, OrderDTO> mapper,
                CancellationToken cancellationToken) =>
            {
                var entities = await orderInterface.GetAllAsync(cancellationToken);
                if (!entities.Any())
                    return Results.NotFound("No order detected in database");

                var orders = mapper.MapList(entities);
                return entities.Any() ? Results.Ok(orders) : Results.NotFound("No orders found");
            })
                .WithName("GetAllOrders")
                .Produces<IEnumerable<OrderDTO>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            group.MapGet("/{id:int}", async (IOrder orderInterface,
                IBaseMapper<Order, OrderDTO> mapper,
                int id,
                CancellationToken cancellationToken) =>
            {
                var entity = await orderInterface.FindByIdAsync(id, cancellationToken);
                if (entity is null)
                    return Results.NotFound("Order not found");

                var order = mapper.MapDTO(entity);
                return Results.Ok(order);
            })
                .WithName("GetProductById")
                .Produces<OrderDTO>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            group.MapGet("/client/{clientId:int}", async ([FromServices] OrderService orderService,
                int clientId,
                CancellationToken cancellationToken) =>
            {
                if (clientId <= 0) return Results.BadRequest("Inavlid data provided");
                var orders = await orderService.GetOrdersByClientId(clientId, cancellationToken);
                return orders.Any() ? Results.Ok(orders) : Results.NotFound(null);
            })
                .WithName("GetClientOrders")
                .Produces<OrderDTO>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest);

            group.MapGet("/details/{orderId:int}", async ([FromServices] OrderService orderService,
                int orderId,
                CancellationToken cancellationToken) =>
            {
                if (orderId <= 0) return Results.BadRequest("Invalid data provided");
                var orderDetail = await orderService.GetOrderDetails(orderId, cancellationToken);
                return orderDetail.OrderId > 0 ? Results.Ok(orderDetail) : Results.NotFound("No order found");
            })
                .WithName("GetOrderDeatails")
                .Produces<OrderDTO>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest);

            group.MapPost("/", async (IOrder orderInterface,
                IBaseMapper<OrderDTO, Order> mapper,
                [FromBody] OrderDTO order,
                CancellationToken cancellationToken) =>
            {
                var entity = mapper.MapDTO(order);
                var response = await orderInterface.CreateAsync(entity, cancellationToken);
                return response.Flag ? Results.Ok(response) : Results.NotFound(response);
            })
                .WithName("CreateOrder")
                .Produces<Response>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            group.MapPut("/", async (IOrder orderInterface,
                IBaseMapper<OrderDTO, Order> mapper,
                [FromBody] OrderDTO order,
                CancellationToken cancellationToken) =>
            {
                var entity = mapper.MapDTO(order);
                var response = await orderInterface.UpdateAsync(entity, cancellationToken);
                return response.Flag ? Results.Ok(response) : Results.BadRequest(response);
            })
                .WithName("UpdateOrder")
                .Produces<Response>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            group.MapDelete("/", async (IOrder orderInterface,
                IBaseMapper<OrderDTO, Order> mapper,
                [FromBody] OrderDTO order,
                CancellationToken cancellationToken) =>
            {
                var entity = mapper.MapDTO(order);
                var response = await orderInterface.DeleteAsync(entity, cancellationToken);
                return response.Flag ? Results.Ok(response) : Results.BadRequest(response);
            })
                .WithName("DeleteOrder")
                .Produces<Response>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
