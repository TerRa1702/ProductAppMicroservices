using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs
{
    public record OrderDTO(int Id,
        [Required] int ProductId,
        [Required] int ClientId,
        [Required] int PurchaseQuantity,
        DateTime OrderedDate);
}
