namespace Infinite.Core.Postgres.Extensions
{
    using Infinite.Core.Postgres.Context;
    using Infinite.Core.Postgres.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration, Assembly configurationAssembly, bool isDesignTime = false)
        {
            string connectionString = configuration.GetValue<string>("PostgresConfig");

            //if (!isDesignTime && string.IsNullOrEmpty(connectionString))
            //    throw new Exception("Postgres connection string not found");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped(typeof(IReadOnlyRepository<>), typeof(ReadOnlyRepository<>));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Registre o Assembly como um singleton para injeção
            services.AddSingleton(configurationAssembly);

            // Registre o ApplicationDbContext com o Assembly
            services.AddScoped<ApplicationDbContext>(provider =>
            {
                var options = provider.GetRequiredService<DbContextOptions<ApplicationDbContext>>();
                var assembly = provider.GetRequiredService<Assembly>();
                return new ApplicationDbContext(options, assembly);
            });

            return services;
        }
    }
}