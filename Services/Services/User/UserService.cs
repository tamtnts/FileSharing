using DataAccess.DTO;
using DataAccess.JWT;
using DataAccess.Repositories.User;

namespace Services.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<string> Register(UserDTO model)
        {
            var user = await _userRepo.GetUserByEmailAsync(model.Email);
            if (user != null)
            {
                throw new ArgumentException("Email already exists.");
            }

            var newUser = new DataAccess.Models.User
            {
                Email = model.Email,
                Password = model.Password, // Hash password in real app
            };

            await _userRepo.AddAsync(newUser);
            await _userRepo.SaveChangesAsync();
            return JWTUserToken.GenerateJwtToken(newUser);
        }

        public async Task<string> Login(UserDTO model)
        {
            var user = await _userRepo.GetUserByEmailAndPasswordAsync(model.Email, model.Password);
            if (user == null)
            {
                throw new ArgumentException("Invalid email or password.");
            }

            return JWTUserToken.GenerateJwtToken(user);
        }
    }
}
