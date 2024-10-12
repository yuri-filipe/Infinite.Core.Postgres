namespace Infinite.Core.Postgres.Extensions
{
    using Infinite.Core.Postgres.Context;
    using Infinite.Core.Postgres.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));
            services.AddScoped(typeof(IReadOnlyRepository<>), typeof(ReadOnlyRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }
    }
}