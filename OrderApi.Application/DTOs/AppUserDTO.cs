using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs
{
    public record AppUserDTO(int Id,
        [Required, StringLength(30, MinimumLength = 3)] string Name,
        [Required, StringLength(15)] string TelephoneNumber,
        [Required] string Address,
        [Required, EmailAddress] string Email,
        [Required, DataType(DataType.Password), StringLength(40, MinimumLength=5)] string Password,
        [Required] string Role);
}
