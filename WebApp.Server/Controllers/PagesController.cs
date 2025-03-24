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
            var pages = await _pageService.GetPagesAsync(); // Senza bisogno di token o userId
            if (pages == null || pages.Count == 0)
            {
                return NotFound(new { message = "Non esistono pagine." });
            }
            return Ok(pages);
        }

        // GET per le sezioni di una pagina
        [HttpGet("{pageId}/sections")]
        public async Task<IActionResult> GetPageSections(Guid pageId)
        {
            var sections = await _pageService.GetPageSectionsAsync(pageId); // Senza bisogno di token o userId
            if (sections == null || sections.Count == 0)
            {
                return NotFound(new { message = "Nessuna sezione trovata per questa pagina." });
            }
            return Ok(sections);
        }

        // GET per i contenuti di una sezione
        [HttpGet("sections/{sectionId}/contents")]
        public async Task<IActionResult> GetPageContents(Guid sectionId)
        {
            var contents = await _pageService.GetPageContentsAsync(sectionId); // Senza bisogno di token o userId
            if (contents == null || contents.Count == 0)
            {
                return NotFound(new { message = "Nessun contenuto trovato per questa sezione." });
            }
            return Ok(contents);
        }

        // Creare una pagina
        [HttpPost]
        public async Task<IActionResult> CreatePage([FromBody] PageDto pageDto)
        {
            if (pageDto == null)
            {
                return BadRequest(new { message = "Dati non validi." });
            }

            // Verifica se l'UserId è valido
            if (!await _pageService.UserExistsAsync(pageDto.UserId))
            {
                return BadRequest(new { message = "L'utente specificato non esiste." });
            }

            var createdPage = await _pageService.CreatePageAsync(pageDto);
            return CreatedAtAction(nameof(GetPages), new { pageId = createdPage.Id }, createdPage);
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
