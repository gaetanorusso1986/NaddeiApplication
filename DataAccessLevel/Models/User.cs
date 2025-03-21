using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Server.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public required string FirstName { get; set; }  // Nome

        [Required]
        [MaxLength(50)]
        public required string LastName { get; set; }   // Cognome

        [Required]
        [MaxLength(50)]
        public required string Username { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string PasswordHash { get; set; }

        [Required]
        public required int RoleId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        public ICollection<PasswordHistory> PasswordHistory { get; set; } = new List<PasswordHistory>();

        [NotMapped]
        public string? AdminAuthCode { get; set; }  // Non viene salvato nel DB, solo per la registrazione
    }
}
