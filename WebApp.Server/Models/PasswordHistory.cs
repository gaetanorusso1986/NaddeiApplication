namespace WebApp.Server.Models
{
    public class PasswordHistory
    {
        public Guid Id { get; set; }  // GUID per PasswordHistory ID
        public Guid UserId { get; set; }  // Collegato a User
        public string PasswordHash { get; set; }  // Hash della password
        public DateTime ChangedAt { get; set; }

        // Relazione con User
        public User User { get; set; }
    }

}
