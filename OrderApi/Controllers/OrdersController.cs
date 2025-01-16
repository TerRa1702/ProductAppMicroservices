using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.DTOs;
using OrderApi.Application.Interfaces;
using OrderApi.Application.Services;
using OrderApi.Domain.Entites;
using SharedLibrary.Interfaces;
using SharedLibrary.Responses;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController(IOrder orderInterface, IOrderService orderService,
        IBaseMapper<Order, OrderDTO> mapTo, IBaseMapper<OrderDTO, Order> mapFrom) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders(CancellationToken cancellationToken)
        {
            var entities = await orderInterface.GetAllAsync(cancellationToken);
            if (!entities.Any())
                return NotFound("No order detected in database");

            var orders = mapTo.MapList(entities);
            return entities.Any() ? Ok(orders) : NotFound("No orders found");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id, CancellationToken cancellationToken)
        {
            var entity = await orderInterface.FindByIdAsync(id, cancellationToken);
            if (entity is null)
                return NotFound("Order not found");

            var order = mapTo.MapDTO(entity);
            return Ok(order);
        }

        [HttpGet("client/{clientId:int}")]
        public async Task<ActionResult<OrderDTO>> GetClientOrders(int clientId, CancellationToken cancellationToken)
        {
            if (clientId <= 0) return BadRequest("Inavlid data provided");
            var orders = await orderService.GetOrdersByClientId(clientId, cancellationToken);
            return orders.Any() ? Ok(orders) : NotFound(null);
        }

        [HttpGet("details/{orderId:int}")]
        public async Task<ActionResult<OrderDetailsDTO>> GetOrderDetails(int orderId, CancellationToken cancellationToken)
        {
            if (orderId <= 0) return BadRequest("Invalid data provided");
            var orderDetail = await orderService.GetOrderDetails(orderId, cancellationToken);
            return orderDetail.OrderId > 0 ? Ok(orderDetail) : NotFound("No order found");
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateOrder(OrderDTO order, CancellationToken cancellationToken)
        {
            if (!ModelState!.IsValid)
                return BadRequest("Incomplete data submitted");

            var entity = mapFrom.MapDTO(order);
            var response = await orderInterface.CreateAsync(entity, cancellationToken);
            return response.Flag ? Ok(response) : NotFound(response);
        }

        [HttpPut]
        public async Task<ActionResult<Response>> UpdateOrder(OrderDTO order, CancellationToken cancellationToken)
        {
            if (!ModelState!.IsValid)
                return BadRequest("Incomplete data submitted");

            var entity = mapFrom.MapDTO(order);
            var response = await orderInterface.UpdateAsync(entity, cancellationToken);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        public async Task<ActionResult<Response>> DeleteOrder(OrderDTO order, CancellationToken cancellationToken)
        {
            var entity = mapFrom.MapDTO(order);
            var response = await orderInterface.DeleteAsync(entity, cancellationToken);
            return response.Flag ? Ok(response) : BadRequest(response);
        }
    }
}
