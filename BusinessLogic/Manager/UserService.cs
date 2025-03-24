using BusinessLogic.Dtos;
using BusinessLogic.IManager;
using DataAccessLevel;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Manager
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Username = u.Username,
                    Email = u.Email,
                    RoleName = u.Role.Name,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();
        }
    }
}
