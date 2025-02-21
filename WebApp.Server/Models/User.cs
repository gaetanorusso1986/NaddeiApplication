using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Server.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }  // GUID per User ID

        [Required]
        [MaxLength(50)]
        public required string Username { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string PasswordHash { get; set; }

        [Required]
        public required int RoleId { get; set; }  // Ruolo è di tipo int

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relazione con Role
        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        // Relazione con PasswordHistory
        public ICollection<PasswordHistory> PasswordHistory { get; set; } = new List<PasswordHistory>();
    }
}
