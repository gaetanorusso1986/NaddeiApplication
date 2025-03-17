using BusinessLogic.Dtos;

namespace BusinessLogic.IManager
{
    public interface IPageService
    {
        Task<List<PageDto>> GetPagesAsync();
        Task<List<PageSectionDto>> GetPageSectionsAsync(Guid pageId);
        Task<List<PageContentDto>> GetPageContentsAsync(Guid sectionId);
        Task<PageDto> CreatePageAsync(PageDto pageDto);
        Task<bool> DeletePageAsync(Guid pageId);
        Task<bool> DeleteAllPagesAsync();
        Task<bool> UserExistsAsync(Guid userId);

    }
}
