using Microsoft.EntityFrameworkCore;
using WebApp.Server.Models;

namespace DataAccessLevel
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<PageSection> PageSections { get; set; }
        public DbSet<PageContent> PageContents { get; set; }
        public DbSet<PasswordHistory> PasswordHistories { get; set; }
        public DbSet<Setting> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Definizione delle relazioni
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PasswordHistory>()
                .HasOne(ph => ph.User)
                .WithMany(u => u.PasswordHistory)
                .HasForeignKey(ph => ph.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Page>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Page>()
                .HasOne(p => p.Parent)
                .WithMany(p => p.Children)
                .HasForeignKey(p => p.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PageSection>()
                .HasOne(ps => ps.Page)
                .WithMany(p => p.PageSections)
                .HasForeignKey(ps => ps.PageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PageContent>()
                .HasOne(pc => pc.PageSection)
                .WithMany(ps => ps.PageContents)
                .HasForeignKey(pc => pc.SectionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed iniziale per i ruoli
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "Admin",
                    CanEditAll = true,
                    CanDeleteAll = true,
                    Description = "Administrator with full permissions, including the ability to edit and delete all pages."
                },
                new Role
                {
                    Id = 2,
                    Name = "User",
                    CanEditAll = false,
                    CanDeleteAll = false,
                    Description = "Standard user with limited permissions, can only edit their own pages."
                }
            );
        }
    }
}
