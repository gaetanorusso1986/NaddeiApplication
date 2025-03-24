namespace WebApp.Server.Models
{
    public class PageContentGroup
    {
        public Guid Id { get; set; }
        public Guid SectionId { get; set; }  // Relazione con PageSection
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int Order { get; set; }
        public PageSection PageSection { get; set; }  // Relazione con PageSection
        public List<PageContent> PageContents { get; set; } = new();
    }


}
