using BusinessLogic.Dtos.WebApp.Server.Dtos;
using BusinessLogic.IManager;
using Microsoft.AspNetCore.Mvc;
using WebApp.Server.Dtos;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { message = "Invalid data", details = ModelState });

        var result = await _authService.RegisterAsync(registerDto);

        if (result != "Success")
            return BadRequest(new { message = result });

        return Ok(new { message = "User registered successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { message = "Invalid data", details = ModelState });

        var token = await _authService.LoginAsync(loginDto);

        if (string.IsNullOrEmpty(token))
            return Unauthorized(new { message = "Invalid email or password." });

        return Ok(new { token });
    }

    [HttpGet("roles")]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _authService.GetRolesAsync();
        return Ok(roles);  // Restituisce i ruoli come RoleDto
    }
}
