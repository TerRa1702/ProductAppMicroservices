using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs
{
    public record OrderDetailsDTO([Required] int OrderId,
        [Required] int ProductId,
        [Required] int Client,
        [Required] string Name,
        [Required, EmailAddress] string Email,
        [Required] string Address,
        [Required, StringLength(15)] string TelephoneNumber,
        [Required, StringLength(100, MinimumLength = 2)] string ProductName,
        [Required] int PurchaseQuantity,
        [Required, DataType(DataType.Currency)] double UnitPrice,
        [Required, DataType(DataType.Currency)] double TotalPrice,
        [Required] DateTime OrderedDate);
}
