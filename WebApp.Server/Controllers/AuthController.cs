﻿using BusinessLogic.Dtos;
using BusinessLogic.IManager;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Server.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                bool isRegistered = await _authService.RegisterAsync(request);
                return Ok(new { message = "Registrazione completata." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _authService.LoginAsync(request);
            if (token == null)
            {
                return Unauthorized(new { message = "Credenziali non valide." });
            }

            return Ok(new { token });
        }


        [HttpGet("password-history/{userId}")]
        public async Task<IActionResult> GetPasswordHistory(Guid userId)
        {
            try
            {
                var passwordHistory = await _authService.GetPasswordHistoryAsync(userId);
                if (passwordHistory == null || !passwordHistory.Any())
                {
                    return NotFound(new { message = "Nessuna cronologia delle password trovata." });
                }
                return Ok(passwordHistory); // Restituisce la lista dei DTO
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
