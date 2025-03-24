namespace WebApp.Server.Models
{
    public class PageContent
    {
        public Guid Id { get; set; }
        public Guid ContentGroupId { get; set; }  // Relazione con PageContentGroup
        public string? ContentType { get; set; }
        public string? ContentData { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int Order { get; set; }

        public PageContentGroup PageContentGroup { get; set; }  // Relazione con PageContentGroup
    }


}


