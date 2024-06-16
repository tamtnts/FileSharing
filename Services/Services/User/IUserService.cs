using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.User
{
    public interface IUserService
    {
        Task<string> Login(UserDTO model);
        Task<string> Register(UserDTO model);
    }
}
