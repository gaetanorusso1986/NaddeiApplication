using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Dtos;
using BusinessLogic.IManager;

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

        // GET: api/pages
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

    }
}
