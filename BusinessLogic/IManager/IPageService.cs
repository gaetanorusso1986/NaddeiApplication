using BusinessLogic.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.IManager
{
    public interface IPageService
    {
        Task<List<PageDto>> GetPagesAsync();
    }
}
