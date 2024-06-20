using DataAccess.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using System;
namespace DataAccess.Repositories.User
{
    public class UserRepository : GenericRepository<Models.User>, IUserRepository
    {
        public UserRepository(FileSharingContext context) : base(context) { }

        public async Task<Models.User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Models.User> GetUserByEmailAndPasswordAsync(string email, string password)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email && u.Password == password);
        }
    }
}
