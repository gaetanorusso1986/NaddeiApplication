using WebApp.Server.Dtos;
using WebApp.Server.Models;

namespace BusinessLogic.IManager
{
    public interface IUserFactory
    {
        User CreateUser(RegisterDto registerDto, string hashedPassword);
    }
}
