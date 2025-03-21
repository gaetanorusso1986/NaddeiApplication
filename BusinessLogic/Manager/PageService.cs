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

        // Chiamate GET per ottenere le pagine
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

        // Chiamata per ottenere le sezioni della pagina
        public async Task<List<PageSectionDto>> GetPageSectionsAsync(Guid pageId)
        {
            var page = await _context.Pages
                .Where(p => p.Id == pageId)
                .FirstOrDefaultAsync();

            if (page == null)
            {
                return null;
            }

            return await _context.PageSections
                .Where(ps => ps.PageId == pageId)
                .Select(ps => new PageSectionDto
                {
                    Id = ps.Id,
                    PageId = ps.PageId,
                    Order = ps.Order,
                    CreatedAt = ps.CreatedAt,
                    PageContents = ps.PageContents.Select(pc => new PageContentDto
                    {
                        Id = pc.Id,
                        SectionId = pc.SectionId,
                        ContentType = pc.ContentType,
                        ContentData = pc.ContentData,
                        CreatedAt = pc.CreatedAt
                    }).ToList()
                })
                .ToListAsync();
        }

        // Chiamata per ottenere i contenuti della sezione della pagina
        public async Task<List<PageContentDto>> GetPageContentsAsync(Guid sectionId)
        {
            var section = await _context.PageSections
                .Where(ps => ps.Id == sectionId)
                .FirstOrDefaultAsync();

            if (section == null)
            {
                return null;
            }

            return await _context.PageContents
                .Where(pc => pc.SectionId == sectionId)
                .Select(pc => new PageContentDto
                {
                    Id = pc.Id,
                    SectionId = pc.SectionId,
                    ContentType = pc.ContentType,
                    ContentData = pc.ContentData,
                    CreatedAt = pc.CreatedAt
                })
                .ToListAsync();
        }

        // Chiamata per creare una nuova pagina
        public async Task<PageDto> CreatePageAsync(PageDto pageDto)
        {
            if (pageDto == null)
            {
                throw new ArgumentNullException(nameof(pageDto));
            }

            // Creiamo una nuova entità Page
            var page = new Page
            {
                Id = Guid.NewGuid(),
                Title = pageDto.Title,
                UserId = pageDto.UserId,
                ParentId = pageDto.ParentId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Aggiungiamo le sezioni, se presenti
            var pageSections = new List<PageSection>();
            foreach (var sectionDto in pageDto.PageSections)
            {
                var section = new PageSection
                {
                    Id = Guid.NewGuid(),
                    PageId = page.Id,
                    Order = sectionDto.Order,
                    CreatedAt = DateTime.UtcNow
                };

                // Aggiungiamo i contenuti, se presenti
                var pageContents = new List<PageContent>();
                foreach (var contentDto in sectionDto.PageContents)
                {
                    var content = new PageContent
                    {
                        Id = Guid.NewGuid(),
                        SectionId = section.Id,
                        ContentType = contentDto.ContentType,
                        ContentData = contentDto.ContentData,
                        CreatedAt = DateTime.UtcNow
                    };
                    pageContents.Add(content);
                }

                section.PageContents = pageContents;
                pageSections.Add(section);
            }

            page.PageSections = pageSections;

            // Salviamo tutto nel database
            _context.Pages.Add(page);
            await _context.SaveChangesAsync();

            // Convertiamo l'entità salvata in DTO per la risposta
            return new PageDto
            {
                Id = page.Id,
                Title = page.Title,
                UserId = page.UserId,
                ParentId = page.ParentId,
                CreatedAt = page.CreatedAt,
                UpdatedAt = page.UpdatedAt,
                PageSections = pageSections.Select(ps => new PageSectionDto
                {
                    Id = ps.Id,
                    PageId = ps.PageId,
                    Order = ps.Order,
                    CreatedAt = ps.CreatedAt,
                    PageContents = ps.PageContents.Select(pc => new PageContentDto
                    {
                        Id = pc.Id,
                        SectionId = pc.SectionId,
                        ContentType = pc.ContentType,
                        ContentData = pc.ContentData,
                        CreatedAt = pc.CreatedAt
                    }).ToList()
                }).ToList()
            };
        }

        // chiamata per vedere se esiste l'utente
        public async Task<bool> UserExistsAsync(Guid userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }

        // Chiamata per eliminare una pagina
        public async Task<bool> DeletePageAsync(Guid pageId)
        {
            var page = await _context.Pages
                .Where(p => p.Id == pageId)
                .Include(p => p.PageSections)
                .ThenInclude(ps => ps.PageContents)
                .FirstOrDefaultAsync();

            if (page == null)
            {
                return false; // La pagina non esiste
            }

            // Rimuove prima i contenuti delle sezioni
            foreach (var section in page.PageSections)
            {
                _context.PageContents.RemoveRange(section.PageContents);
            }

            // Rimuove le sezioni della pagina
            _context.PageSections.RemoveRange(page.PageSections);

            // Rimuove la pagina
            _context.Pages.Remove(page);

            await _context.SaveChangesAsync();
            return true;
        }

        // Chiamata per eliminare tutte le pagine
        public async Task<bool> DeleteAllPagesAsync()
        {
            var pages = await _context.Pages
                .Include(p => p.PageSections)
                .ThenInclude(ps => ps.PageContents)
                .ToListAsync();

            if (pages == null || pages.Count == 0)
            {
                return false; // Nessuna pagina trovata da eliminare
            }

            // Rimuoviamo prima i contenuti di tutte le sezioni
            foreach (var page in pages)
            {
                foreach (var section in page.PageSections)
                {
                    _context.PageContents.RemoveRange(section.PageContents);
                }
            }

            // Rimuoviamo le sezioni di tutte le pagine
            foreach (var page in pages)
            {
                _context.PageSections.RemoveRange(page.PageSections);
            }

            // Rimuoviamo le pagine
            _context.Pages.RemoveRange(pages);

            await _context.SaveChangesAsync(); // Salviamo le modifiche

            return true;
        }
    }
}
