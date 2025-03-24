using BusinessLogic.Dtos;

namespace BusinessLogic.IManager
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
    }
}
