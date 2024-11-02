namespace Infinite.Core.Postgres.Context
{
    using Infinite.Core.Postgres.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using System.Linq.Expressions;
    using System.Reflection;
    public class ApplicationDbContext : DbContext
    {
        private readonly Assembly _configurationAssembly;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, Assembly configurationAssembly)
            : base(options)
        {
            _configurationAssembly = configurationAssembly;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(_configurationAssembly);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var propInfo = entityType.ClrType.GetProperty("Excluido");

                    if (propInfo != null && propInfo.PropertyType == typeof(bool))
                    {
                        var propAccess = Expression.MakeMemberAccess(parameter, propInfo);
                        var compareExpression = Expression.Equal(propAccess, Expression.Constant(false));
                        var lambda = Expression.Lambda(compareExpression, parameter);

                        modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                    }
                }

                var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc),    // Conversão para o banco de dados (para UTC)
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)     // Conversão ao ler do banco (como UTC)
                );

                var dateTimeProperties = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?));

                foreach (var property in dateTimeProperties)
                {
                    modelBuilder.Entity(entityType.ClrType).Property(property.Name)
                        .HasConversion(dateTimeConverter);
                }
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<CoreEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(e => e.DataInclusao).CurrentValue = DateTime.UtcNow;
                    entry.Property(e => e.UsuarioInclusao).CurrentValue = GetCurrentUser(); // Implement this method
                    entry.Property(e => e.DataAlteracao).CurrentValue = DateTime.UtcNow;
                    entry.Property(e => e.UsuarioAlteracao).CurrentValue = GetCurrentUser(); // Implement this method
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(e => e.DataAlteracao).CurrentValue = DateTime.UtcNow;
                    entry.Property(e => e.UsuarioAlteracao).CurrentValue = GetCurrentUser(); // Implement this method

                    entry.Property(e => e.DataInclusao).IsModified = false;
                    entry.Property(e => e.UsuarioInclusao).IsModified = false;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        private string GetCurrentUser()
        {
            // Implement logic to retrieve the current user's identifier
            // For example, from the HTTP context or a service
            return "-"; // Placeholder value
        }
    }
}
