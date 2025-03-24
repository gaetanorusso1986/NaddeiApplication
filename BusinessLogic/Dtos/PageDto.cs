namespace BusinessLogic.Dtos
{
    public class PageDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public Guid UserId { get; set; }
        public Guid? ParentId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public List<PageSectionDto> PageSections { get; set; } = new();
    }

    public class PageSectionDto
    {
        public Guid Id { get; set; }
        public Guid PageId { get; set; }
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<PageContentGroupDto> PageContentGroups { get; set; } = new();
    }

    public class PageContentGroupDto
    {
        public Guid Id { get; set; }
        public Guid SectionId { get; set; } // Riferisce a PageSection
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<PageContentDto> PageContents { get; set; } = new();
    }

    public class PageContentDto
    {
        public Guid Id { get; set; }
        public Guid ContentGroupId { get; set; }  // Riferisce a PageContentGroup
        public string? ContentType { get; set; }
        public string? ContentData { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int Order { get; set; } // Ordine all'interno del ContentGroup
    }
}
