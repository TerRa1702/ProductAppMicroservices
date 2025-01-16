using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs
{
    public record ProductDTO(int Id,
        [Required, StringLength(100, MinimumLength = 2)] string Name,
        [Required, Range(1, int.MaxValue)] int Quantity,
        [Required, DataType(DataType.Currency)] double Price);
}
