using BusinessLogic.Dtos;
using BusinessLogic.IManager;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using WebApp.Server.Models;

namespace WebApp.Server.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _usersService.GetAllUsersAsync();
            return Ok(users);
        }


        [HttpGet("roles/names")]
        public async Task<IActionResult> GetRoleNames()
        {
            var roleNames = await _usersService.GetRoleNamesAsync();  // Make sure it’s assigned properly
            return Ok(roleNames);
        }



        // Aggiungi questo metodo al tuo controller UsersController
        [HttpDelete]
        public async Task<IActionResult> DeleteAllUsers()
        {
            await _usersService.DeleteAllUsersAsync();
            return NoContent();  // 204 No Content
        }


    }
}
