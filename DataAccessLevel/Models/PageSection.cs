namespace WebApp.Server.Models
{
    public class PageSection
    {
        public Guid Id { get; set; }
        public Guid PageId { get; set; }  // Relazione con Page
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Aggiungi questa proprietà di navigazione per stabilire la relazione inversa
        public Page Page { get; set; }  // Relazione con Page
        public List<PageContentGroup> PageContentGroups { get; set; } = new();
    }

}
