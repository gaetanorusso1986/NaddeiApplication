
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

        // chiamata per vedere se esiste l'utente
        public async Task<bool> UserExistsAsync(Guid userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }



        // Chiamate GET per ottenere le pagine
        public async Task<List<PageDto>> GetPagesAsync()
        {
            var pages = await _context.Pages
                .Include(p => p.PageSections)
                    .ThenInclude(ps => ps.PageContentGroups)
                        .ThenInclude(pcg => pcg.PageContents)
                .Select(p => new PageDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    UserId = p.UserId,
                    ParentId = p.ParentId,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    PageSections = p.PageSections.Select(ps => new PageSectionDto
                    {
                        Id = ps.Id,
                        PageId = ps.PageId,
                        Order = ps.Order,
                        CreatedAt = ps.CreatedAt,
                        PageContentGroups = ps.PageContentGroups.Select(pcg => new PageContentGroupDto
                        {
                            Id = pcg.Id,
                            SectionId = pcg.SectionId,
                            Order = pcg.Order,
                            CreatedAt = pcg.CreatedAt,
                            PageContents = pcg.PageContents.Select(pc => new PageContentDto
                            {
                                Id = pc.Id,
                                ContentGroupId = pc.ContentGroupId,
                                ContentType = pc.ContentType,
                                ContentData = pc.ContentData,
                                CreatedAt = pc.CreatedAt,
                                Order = pc.Order
                            }).ToList()
                        }).ToList()
                    }).ToList()
                })
                .ToListAsync();

            return pages;
        }



        // Chiamata per creare una nuova pagina
        public async Task<PageDto> CreatePageAsync(PageDto pageDto)
        {
            if (pageDto == null)
            {
                throw new ArgumentNullException(nameof(pageDto));
            }

            var page = new Page
            {
                Id = Guid.NewGuid(),
                Title = pageDto.Title,
                UserId = pageDto.UserId,
                ParentId = pageDto.ParentId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

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

                var pageContentGroups = new List<PageContentGroup>();
                foreach (var contentGroupDto in sectionDto.PageContentGroups)
                {
                    var contentGroup = new PageContentGroup
                    {
                        Id = Guid.NewGuid(),
                        SectionId = section.Id,
                        Order = contentGroupDto.Order,
                        CreatedAt = DateTime.UtcNow
                    };

                    var pageContents = contentGroupDto.PageContents.Select(contentDto => new PageContent
                    {
                        Id = Guid.NewGuid(),
                        ContentGroupId = contentGroup.Id,
                        ContentType = contentDto.ContentType,
                        ContentData = contentDto.ContentData,
                        CreatedAt = DateTime.UtcNow,
                        Order = contentDto.Order
                    }).ToList();

                    contentGroup.PageContents = pageContents;
                    pageContentGroups.Add(contentGroup);
                }

                section.PageContentGroups = pageContentGroups;
                pageSections.Add(section);
            }

            page.PageSections = pageSections;

            _context.Pages.Add(page);
            await _context.SaveChangesAsync();

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
                    PageContentGroups = ps.PageContentGroups.Select(pcg => new PageContentGroupDto
                    {
                        Id = pcg.Id,
                        SectionId = pcg.SectionId,
                        Order = pcg.Order,
                        CreatedAt = pcg.CreatedAt,
                        PageContents = pcg.PageContents.Select(pc => new PageContentDto
                        {
                            Id = pc.Id,
                            ContentGroupId = pc.ContentGroupId,
                            ContentType = pc.ContentType,
                            ContentData = pc.ContentData,
                            CreatedAt = pc.CreatedAt,
                            Order = pc.Order
                        }).ToList()
                    }).ToList()
                }).ToList()
            };
        }

        // Chiamata per creare una nuova sezione
        public async Task<PageSectionDto> CreatePageSectionAsync(Guid pageId, PageSectionDto sectionDto)
        {
            var page = await _context.Pages.Include(p => p.PageSections).FirstOrDefaultAsync(p => p.Id == pageId);
            if (page == null) throw new ArgumentException("Page not found");

            int nextOrder = page.PageSections.Any() ? page.PageSections.Max(s => s.Order) + 1 : 1;

            var section = new PageSection
            {
                Id = Guid.NewGuid(),
                PageId = pageId,
                Order = nextOrder,
                CreatedAt = DateTime.UtcNow
            };

            _context.PageSections.Add(section);
            await _context.SaveChangesAsync();

            return new PageSectionDto
            {
                Id = section.Id,
                PageId = section.PageId,
                Order = section.Order,
                CreatedAt = section.CreatedAt
            };
        }

        //Chiamata per creare una nuovo gruppo 
        public async Task<PageContentGroupDto> CreatePageContentGroupAsync(Guid sectionId, PageContentGroupDto contentGroupDto)
        {
            var section = await _context.PageSections.Include(s => s.PageContentGroups).FirstOrDefaultAsync(s => s.Id == sectionId);
            if (section == null) throw new ArgumentException("Section not found");

            int nextOrder = section.PageContentGroups.Any() ? section.PageContentGroups.Max(g => g.Order) + 1 : 1;

            var contentGroup = new PageContentGroup
            {
                Id = Guid.NewGuid(),
                SectionId = sectionId,
                Order = nextOrder,
                CreatedAt = DateTime.UtcNow
            };

            _context.PageContentGroups.Add(contentGroup);
            await _context.SaveChangesAsync();

            return new PageContentGroupDto
            {
                Id = contentGroup.Id,
                SectionId = contentGroup.SectionId,
                Order = contentGroup.Order,
                CreatedAt = contentGroup.CreatedAt
            };
        }

        //Chiamata per creare nuovi contenuti 
        public async Task<PageContentDto> CreatePageContentAsync(Guid contentGroupId, PageContentDto contentDto)
        {
            var contentGroup = await _context.PageContentGroups.Include(cg => cg.PageContents).FirstOrDefaultAsync(cg => cg.Id == contentGroupId);
            if (contentGroup == null) throw new ArgumentException("Content Group not found");

            int nextOrder = contentGroup.PageContents.Any() ? contentGroup.PageContents.Max(c => c.Order) + 1 : 1;

            var content = new PageContent
            {
                Id = Guid.NewGuid(),
                ContentGroupId = contentGroupId,
                ContentType = contentDto.ContentType,
                ContentData = contentDto.ContentData,
                CreatedAt = DateTime.UtcNow,
                Order = nextOrder
            };

            _context.PageContents.Add(content);
            await _context.SaveChangesAsync();

            return new PageContentDto
            {
                Id = content.Id,
                ContentGroupId = content.ContentGroupId,
                ContentType = content.ContentType,
                ContentData = content.ContentData,
                CreatedAt = content.CreatedAt,
                Order = content.Order
            };
        }



        // Chiamata per eliminare una pagina
        public async Task<bool> DeletePageAsync(Guid pageId)
        {
            var page = await _context.Pages
                .Where(p => p.Id == pageId)
                .Include(p => p.PageSections)
                .ThenInclude(ps => ps.PageContentGroups)
                .ThenInclude(pcg => pcg.PageContents)
                .FirstOrDefaultAsync();

            if (page == null)
            {
                return false; // La pagina non esiste
            }

            // Rimuove prima i contenuti dei gruppi di contenuti
            foreach (var section in page.PageSections)
            {
                foreach (var contentGroup in section.PageContentGroups)
                {
                    _context.PageContents.RemoveRange(contentGroup.PageContents);
                }
            }

            // Rimuove i gruppi di contenuti delle sezioni
            foreach (var section in page.PageSections)
            {
                _context.PageContentGroups.RemoveRange(section.PageContentGroups);
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
                .ThenInclude(ps => ps.PageContentGroups)
                .ThenInclude(pcg => pcg.PageContents)
                .ToListAsync();

            if (pages == null || pages.Count == 0)
            {
                return false; // Nessuna pagina trovata da eliminare
            }

            // Rimuoviamo prima i contenuti di tutti i gruppi di contenuti
            foreach (var page in pages)
            {
                foreach (var section in page.PageSections)
                {
                    foreach (var contentGroup in section.PageContentGroups)
                    {
                        _context.PageContents.RemoveRange(contentGroup.PageContents);
                    }
                }
            }

            // Rimuoviamo i gruppi di contenuti di tutte le sezioni
            foreach (var page in pages)
            {
                foreach (var section in page.PageSections)
                {
                    _context.PageContentGroups.RemoveRange(section.PageContentGroups);
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
