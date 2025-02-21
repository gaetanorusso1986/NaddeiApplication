namespace WebApp.Server.Models
{
    public class Setting
    {
        public Guid Id { get; set; }  // GUID per Setting ID
        public string Key { get; set; }  // Chiave dell'impostazione
        public string Value { get; set; }  // Valore dell'impostazione
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
