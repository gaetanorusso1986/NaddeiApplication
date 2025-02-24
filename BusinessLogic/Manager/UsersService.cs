using BusinessLogic.Dtos;
using BusinessLogic.IManager;
using DataAccessLevel;
using Microsoft.EntityFrameworkCore;
using WebApp.Server.Models;

namespace BusinessLogic.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext _context;

        public UsersService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            return await _context.Users
                .Select(u => new UserDto
                {
                    Username = u.Username,
                    Email = u.Email,
                    Role = u.Role != null ? u.Role.Name : "Nessun ruolo"
                })
                .ToListAsync();
        }
        public async Task DeleteAllUsersAsync()
        {
            var users = _context.Users.ToList();
            _context.Users.RemoveRange(users);
            await _context.SaveChangesAsync();
        }

        public async Task<List<string>> GetRoleNamesAsync()
        {
            return await _context.Roles.Select(r => r.Name).ToListAsync();
        }


    }
}
