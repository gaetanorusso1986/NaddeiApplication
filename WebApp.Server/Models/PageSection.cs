namespace WebApp.Server.Models
{
    public class PageSection
    {
        public Guid Id { get; set; }  // GUID per PageSection ID
        public Guid PageId { get; set; }  // Collegato a Page
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; }

        // Relazione con Page
        public Page Page { get; set; }

        // Relazione con PageContents
        public ICollection<PageContent> PageContents { get; set; }
    }

}
