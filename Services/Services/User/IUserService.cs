using DataAccess.DTO;

namespace Services.Services.User
{
    public interface IUserService
    {
        Task<string> Login(UserDTO model);
        Task<string> Register(UserDTO model);
    }
}
