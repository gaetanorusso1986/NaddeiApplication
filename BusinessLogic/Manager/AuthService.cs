using BusinessLogic.Dtos.WebApp.Server.Dtos;
using BusinessLogic.Dtos;
using BusinessLogic.IManager;
using DataAccessLevel;
using System.Text;
using WebApp.Server.Dtos;
using Microsoft.EntityFrameworkCore;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly IUserFactory _userFactory;

    public AuthService(ApplicationDbContext context, ITokenService tokenService, IUserFactory userFactory)
    {
        _context = context;
        _tokenService = tokenService;
        _userFactory = userFactory;
    }

    public async Task<string> RegisterAsync(RegisterDto registerDto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            return "Email already in use";

        if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
            return "Username already taken";

        var roleExists = await _context.Roles.AnyAsync(r => r.Id == registerDto.RoleId);
        if (!roleExists)
            return "Invalid role";

        var hashedPassword = HashPassword(registerDto.Password);
        var user = _userFactory.CreateUser(registerDto, hashedPassword);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return "Success";
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
        if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
            return "Invalid email or password";

        return _tokenService.GenerateJwtToken(user);
    }

    public async Task<IEnumerable<RoleDto>> GetRolesAsync()
    {
        var roles = await _context.Roles
            .Select(r => new RoleDto
            {
                Id = r.Id.ToString(),  
                Name = r.Name
            })
            .ToListAsync();

        return roles;
    }


    private string HashPassword(string password) =>
        Convert.ToBase64String(System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password)));

    private bool VerifyPassword(string password, string storedHash) =>
        HashPassword(password) == storedHash;
}
