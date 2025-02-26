
using BusinessLogic.Dtos;
using BusinessLogic.IManager;
using DataAccessLevel;
using Microsoft.EntityFrameworkCore;
using WebApp.Server.Models;

namespace BusinessLogic.Manager
{
    public class PageService : IPageService
    {
        private readonly ApplicationDbContext _context;

        public PageService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Ottieni tutte le pagine
        public async Task<List<PageDto>> GetPagesAsync()
        {
            var pages = await _context.Pages
                .Select(p => new PageDto
                {
                    Id = p.Id,
                    Title = p.Title
                })
                .ToListAsync();

            return pages;
        }

    }
}
