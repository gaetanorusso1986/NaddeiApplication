using BusinessLogic.Dtos;
using BusinessLogic.IManager;
using DataAccessLevel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApp.Server.Models;

namespace BusinessLogic.Manager
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly string _adminAuthCode;
        private readonly string _jwtSecret;
        private readonly int _jwtExpirationMinutes;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _adminAuthCode = configuration["Authentication:AdminAuthCode"]
                ?? throw new Exception("Admin authentication code not found in configuration.");
            _jwtSecret = configuration["Jwt:Secret"]
                ?? throw new Exception("JWT secret key not found in configuration.");
            _jwtExpirationMinutes = int.Parse(configuration["Jwt:ExpirationMinutes"] ?? "60");
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                throw new Exception("Email già in uso.");
            }

            var role = await _context.Roles.FindAsync(request.RoleId);
            if (role == null)
            {
                throw new Exception("Ruolo non valido.");
            }

            if (role.Name == "Admin" && request.AdminAuthCode != _adminAuthCode)
            {
                throw new Exception("Codice di autenticazione admin non valido.");
            }

            // Creazione dell'utente
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                RoleId = request.RoleId
            };

            // Aggiungi l'utente al contesto del database
            _context.Users.Add(user);

            // Salva la password nella PasswordHistory
            var passwordHistory = new PasswordHistory
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                PasswordHash = user.PasswordHash,
                ChangedAt = DateTime.UtcNow
            };

            _context.PasswordHistories.Add(passwordHistory);

            // Salva entrambe le entità nel database
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<string?> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return null;
            }

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.Name)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<IEnumerable<PasswordHistoryDto>> GetPasswordHistoryAsync(Guid userId)
        {
            var passwordHistory = await _context.PasswordHistories
                .Where(ph => ph.UserId == userId)
                .OrderByDescending(ph => ph.ChangedAt)
                .Include(ph => ph.User) // Assicurati di includere User
                .ToListAsync();

            // Proietta i dati sul DTO
            var passwordHistoryDtos = passwordHistory.Select(ph => new PasswordHistoryDto
            {
                FirstName = ph.User.FirstName,
                LastName = ph.User.LastName,
                Email = ph.User.Email,
                ChangedAt = ph.ChangedAt,
                PasswordHash = ph.PasswordHash  // Ritorna la password non criptata
            });

            return passwordHistoryDtos;
        }


    }
}
