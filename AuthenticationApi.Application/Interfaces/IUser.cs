using AuthenticationApi.Application.DTOs;
using SharedLibrary.Responses;

namespace AuthenticationApi.Application.Interfaces
{
    public interface IUser
    {
        public Task<Response> Register(AppUserDTO appUserDTO);
        public Task<Response> Login(LoginDTO loginDTO);
        public Task<GetUserDTO> GetUser(int userId);
    }
}
