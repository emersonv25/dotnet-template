using Microsoft.EntityFrameworkCore;
using Template.Domain.Entities;
using Template.Data.Mappings;

namespace Template.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                                .Property("Id")
                                .HasDefaultValueSql("uuid_generate_v4()");

                    modelBuilder.Entity(entityType.ClrType)
                                .Property("CreatedAt")
                                .HasDefaultValueSql("CURRENT_TIMESTAMP");
                }
            }

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            modelBuilder.HasPostgresExtension("uuid-ossp");
        }

    }
}
