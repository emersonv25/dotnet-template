using Template.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Template.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().HasIndex(u => new { u.Email, u.FirebaseUid }).IsUnique();
            modelBuilder.Entity<User>().Property(u =>  u.Name).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Email).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.FirebaseUid).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Email).HasMaxLength(255);
            modelBuilder.Entity<User>().Property(e => e.FirebaseUid).HasMaxLength(28);
        }
    }
}

