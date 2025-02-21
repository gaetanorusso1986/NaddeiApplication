namespace WebApp.Server.Models
{
    public class PageContent
    {
        public Guid Id { get; set; }  // GUID per PageContent ID
        public Guid SectionId { get; set; }  // Collegato a PageSection
        public string ContentType { get; set; }  // Enum per tipo di contenuto (text, image, file, video)
        public string ContentData { get; set; }  // Dati del contenuto
        public DateTime CreatedAt { get; set; }

        // Relazione con PageSection
        public PageSection PageSection { get; set; }
    }

}
