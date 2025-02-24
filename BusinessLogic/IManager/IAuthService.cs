using BusinessLogic.Dtos;
using BusinessLogic.Dtos.WebApp.Server.Dtos;
using WebApp.Server.Dtos;


namespace BusinessLogic.IManager
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto registerDto);
        Task<string> LoginAsync(LoginDto loginDto);
        Task<IEnumerable<RoleDto>> GetRolesAsync();  // Aggiungi questo metodo
    }

}
