using BusinessLogic.Dtos;

namespace BusinessLogic.IManager
{
    public interface IUsersService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task DeleteAllUsersAsync();
        Task<List<string>> GetRoleNamesAsync();  // Update return type to Task<List<string>>
    }
}
