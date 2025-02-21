namespace WebApp.Server.Models
{
    public class Role
    {
        public int Id { get; set; }  // Role ID è di tipo int
        public string Name { get; set; }
        public bool CanEditAll { get; set; }
        public bool CanDeleteAll { get; set; }
        public string Description { get; set; }

        // Relazione con Users
        public ICollection<User> Users { get; set; }
    }

}
