using BusinessLogic.Dtos;

namespace BusinessLogic.IManager
{
    public interface IPageService
    {
        Task<List<PageDto>> GetPagesAsync();
        Task<PageDto> CreatePageAsync(PageDto pageDto);
        Task<PageSectionDto> CreatePageSectionAsync(Guid pageId, PageSectionDto sectionDto);
        Task<PageContentGroupDto> CreatePageContentGroupAsync(Guid sectionId, PageContentGroupDto contentGroupDto);
        Task<PageContentDto> CreatePageContentAsync(Guid contentGroupId, PageContentDto contentDto);
        Task<bool> DeletePageAsync(Guid pageId);
        Task<bool> DeleteAllPagesAsync();
        Task<bool> UserExistsAsync(Guid userId);

    }
}
