using BusinessLogic.IManager;
using WebApp.Server.Dtos;
using WebApp.Server.Models;

namespace BusinessLogic.Manager
{
    public class UserFactory : IUserFactory
    {
        public User CreateUser(RegisterDto registerDto, string hashedPassword)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = hashedPassword,
                RoleId = registerDto.RoleId,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
