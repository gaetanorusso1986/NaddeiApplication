namespace BusinessLogic.Dtos
{
    public class PasswordHistoryDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime ChangedAt { get; set; }
        public string PasswordHash { get; set; }  // Password non criptata
    }
}
