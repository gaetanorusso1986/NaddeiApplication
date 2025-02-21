using Microsoft.EntityFrameworkCore;
using WebApp.Server.Models;

namespace WebApp.Server.Data
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
            base.OnModelCreating(modelBuilder);

            // Impostazioni di base per GUID
            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Page>()
                .Property(p => p.Id)
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<PageSection>()
                .Property(ps => ps.Id)
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<PageContent>()
                .Property(pc => pc.Id)
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<PasswordHistory>()
                .Property(ph => ph.Id)
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Setting>()
                .Property(s => s.Id)
                .HasDefaultValueSql("NEWID()");

            // Aggiungere i ruoli Admin e User con descrizioni
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
