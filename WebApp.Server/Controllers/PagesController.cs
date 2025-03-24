using Microsoft.AspNetCore.Mvc;
using BusinessLogic.IManager;
using BusinessLogic.Dtos;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace WebApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagesController : ControllerBase
    {
        private readonly IPageService _pageService;

        public PagesController(IPageService pageService)
        {
            _pageService = pageService;
        }

        // GET per le pagine
        [HttpGet]
        public async Task<IActionResult> GetPages()
        {
            var pages = await _pageService.GetPagesAsync();
            if (pages == null || pages.Count == 0)
            {
                return NotFound(new { message = "Non esistono pagine." });
            }
            return Ok(pages);
        }



        // Creare una pagina
        [HttpPost]
        public async Task<IActionResult> CreatePage([FromBody] PageDto pageDto)
        {
            if (pageDto == null)
            {
                return BadRequest(new { message = "Dati non validi." });
            }

            if (!await _pageService.UserExistsAsync(pageDto.UserId))
            {
                return BadRequest(new { message = "L'utente specificato non esiste." });
            }

            var createdPage = await _pageService.CreatePageAsync(pageDto);
            return CreatedAtAction(nameof(GetPages), new { pageId = createdPage.Id }, createdPage);
        }

        // Creare una sezione della pagina
        [HttpPost("{pageId}/sections")]
        public async Task<IActionResult> CreatePageSection(Guid pageId, [FromBody] PageSectionDto sectionDto)
        {
            if (sectionDto == null)
            {
                return BadRequest(new { message = "Dati non validi." });
            }

            var createdSection = await _pageService.CreatePageSectionAsync(pageId, sectionDto);
            return CreatedAtAction(nameof(GetPages), new { pageId = createdSection.PageId, sectionId = createdSection.Id }, createdSection);
        }

        // Creare un gruppo di contenuti all'interno di una sezione
        [HttpPost("sections/{sectionId}/content-groups")]
        public async Task<IActionResult> CreatePageContentGroup(Guid sectionId, [FromBody] PageContentGroupDto contentGroupDto)
        {
            if (contentGroupDto == null)
            {
                return BadRequest(new { message = "Dati non validi." });
            }

            var createdContentGroup = await _pageService.CreatePageContentGroupAsync(sectionId, contentGroupDto);
            return CreatedAtAction(nameof(GetPages), new { sectionId = createdContentGroup.SectionId, contentGroupId = createdContentGroup.Id }, createdContentGroup);
        }

        // Creare un contenuto all'interno di un gruppo di contenuti
        [HttpPost("content-groups/{contentGroupId}/contents")]
        public async Task<IActionResult> CreatePageContent(Guid contentGroupId, [FromBody] PageContentDto contentDto)
        {
            if (contentDto == null)
            {
                return BadRequest(new { message = "Dati non validi." });
            }

            var createdContent = await _pageService.CreatePageContentAsync(contentGroupId, contentDto);
            return CreatedAtAction(nameof(GetPages), new { contentGroupId = createdContent.ContentGroupId, contentId = createdContent.Id }, createdContent);
        }



        // Eliminare una pagina
        [HttpDelete("{pageId}")]
        public async Task<IActionResult> DeletePage(Guid pageId)
        {
            var deleted = await _pageService.DeletePageAsync(pageId);
            if (!deleted)
            {
                return NotFound(new { message = "Pagina non trovata o già eliminata." });
            }

            return NoContent();
        }

        // Eliminare tutte le pagine
        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAllPages()
        {
            var deleted = await _pageService.DeleteAllPagesAsync();
            if (!deleted)
            {
                return NotFound(new { message = "Nessuna pagina trovata da eliminare." });
            }

            return NoContent();
        }
    }
}
