
using BusinessLogic.Dtos;

namespace BusinessLogic.IManager
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterRequest request);
        Task<string?> LoginAsync(LoginRequest request);
    }
}
