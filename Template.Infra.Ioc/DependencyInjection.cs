using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Template.Application.Interfaces;
using Template.Application.Services;
using Template.Data.Repositories;
using Template.Data;
using Template.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Template.Infra.Data.Services;
using Template.Domain.Interfaces.Services;

namespace Template.Infra.Ioc
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuração PostgreSQL
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
            });


            //Repositories
            services.AddScoped<IUserRepository, UserRepository>();

            //Services
            services.AddScoped<IUserService, UserService>();

            // Firebase
            services.AddSingleton<IFirebaseService, FirebaseService>();

            //Cache
            services.AddMemoryCache();

            return services;
        }
    }
}
