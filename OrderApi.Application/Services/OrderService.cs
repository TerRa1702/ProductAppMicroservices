using OrderApi.Application.DTOs;
using OrderApi.Application.Interfaces;
using OrderApi.Domain.Entites;
using Polly.Registry;
using SharedLibrary.Interfaces;
using System.Net.Http.Json;

namespace OrderApi.Application.Services
{
    public class OrderService(IOrder orderInterface,
        HttpClient httpClient,
        ResiliencePipelineProvider<string> resiliencePipeline,
        IBaseMapper<Order, OrderDTO> mapper) : IOrderService
    {
        public async Task<ProductDTO> GetProduct(int productId)
        {
            var getProduct = await httpClient.GetAsync($"/api/products/{productId}");
            if (!getProduct.IsSuccessStatusCode)
                return null!;

            var product = await getProduct.Content.ReadFromJsonAsync<ProductDTO>();
            return product!;
        }

        public async Task<AppUserDTO> GetUser(int userId)
        {
            var getUser = await httpClient.GetAsync($"api/authentication/{userId}");
            if (!getUser.IsSuccessStatusCode)
                return null!;

            var user = await getUser.Content.ReadFromJsonAsync<AppUserDTO>();
            return user!;
        }

        public async Task<OrderDetailsDTO> GetOrderDetails(int orderId, CancellationToken cancellationToken)
        {
            var order = await orderInterface.FindByIdAsync(orderId, cancellationToken);
            if (order is null || order.Id <= 0)
                return null!;

            var retryPipline = resiliencePipeline.GetPipeline("my-retry-pipeline");
            var productDTO = await retryPipline.ExecuteAsync(async token => await GetProduct(order.ProductId));
            var appUserDTO = await retryPipline.ExecuteAsync(async token => await GetUser(order.ClientId));

            return new OrderDetailsDTO(order.Id,
                productDTO.Id,
                appUserDTO.Id,
                appUserDTO.Name,
                appUserDTO.Email,
                appUserDTO.Address,
                appUserDTO.TelephoneNumber,
                productDTO.Name,
                order.PurchaseQuantity,
                productDTO.Price,
                productDTO.Quantity * order.PurchaseQuantity,
                order.OrderedDate);
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByClientId(int clientId, CancellationToken cancellationToken)
        {
            var entities = await orderInterface.GetOrdersAsync(o => o.ClientId == clientId);
            if (!entities.Any()) return null!;

            var orders = mapper.MapList(entities);
            return orders;
        }
    }
}
