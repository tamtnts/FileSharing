using DataAccess.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.User
{
    public interface IUserRepository : IGenericRepository<Models.User>
    {
        Task<Models.User> GetUserByEmailAsync(string email);
        Task<Models.User> GetUserByEmailAndPasswordAsync(string email, string password);
    }
}
