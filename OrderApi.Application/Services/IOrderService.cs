using OrderApi.Application.DTOs;

namespace OrderApi.Application.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetOrdersByClientId(int clientId, CancellationToken cancellationToken);
        Task<OrderDetailsDTO> GetOrderDetails(int orderId, CancellationToken cancellationToken);
    }
}
