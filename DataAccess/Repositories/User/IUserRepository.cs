using DataAccess.Repositories.Generic;
namespace DataAccess.Repositories.User
{
    public interface IUserRepository : IGenericRepository<Models.User>
    {
        Task<Models.User> GetUserByEmailAsync(string email);
        Task<Models.User> GetUserByEmailAndPasswordAsync(string email, string password);
    }
}
