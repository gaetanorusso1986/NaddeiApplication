
using BusinessLogic.Dtos;
using WebApp.Server.Models;

namespace BusinessLogic.IManager
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterRequest request);
        Task<string?> LoginAsync(LoginRequest request);
        Task<IEnumerable<PasswordHistoryDto>> GetPasswordHistoryAsync(Guid userId);

    }
}
