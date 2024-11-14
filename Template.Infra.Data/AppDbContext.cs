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

            // Aplica as configurações para todas as entidades concretas que herdam de BaseEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Verifica se a entidade herda de BaseEntity
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    // Configura o Id como Guid com valor padrão gerado pelo banco
                    modelBuilder.Entity(entityType.ClrType)
                                .Property("Id")
                                .HasDefaultValueSql("uuid_generate_v4()");

                    // Configura a propriedade CreatedAt com o valor padrão CURRENT_TIMESTAMP
                    modelBuilder.Entity(entityType.ClrType)
                                .Property("CreatedAt")
                                .HasDefaultValueSql("CURRENT_TIMESTAMP");
                }
            }


            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity && e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;
                entity.UpdatedAt = DateTime.UtcNow; // Atualiza apenas o UpdatedAt
            }

            return base.SaveChanges();
        }
    }
}
