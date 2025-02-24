namespace WebApp.Server.Models
{
    public class Page
    {
        public Guid Id { get; set; }  // GUID per Page ID
        public string Title { get; set; }
        public Guid UserId { get; set; }  // Creatore della pagina, collegato a User
        public Guid? ParentId { get; set; }  // Pagina padre (nullable)
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Relazione con User
        public User User { get; set; }

        // Relazione con Pages (Parent-Child)
        public Page Parent { get; set; }
        public ICollection<Page> Children { get; set; }

        // Relazione con PageSections
        public ICollection<PageSection> PageSections { get; set; }
    }

}
