using System.ComponentModel.DataAnnotations;

namespace AuthenticationApi.Application.DTOs
{
    public record GetUserDTO(int Id,
        [Required, StringLength(30, MinimumLength = 3)] string Name,
        [Required, StringLength(15)] string TelephoneNumber,
        [Required] string Address,
        [Required, EmailAddress] string Email,
        [Required] string Role);
}
