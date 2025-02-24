
using WebApp.Server.Models;

namespace BusinessLogic.IManager
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
    }
}
