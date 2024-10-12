using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infinite.Core.Postgres.Mapping
{
    public abstract class CoreViewMapping<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : CoreView<TKey>
    {
        protected abstract string Schema { get; }
        protected abstract string ViewName { get; }

        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.ToView(ViewName, Schema);

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("ID")
                .HasColumnOrder(0);

            ConfigureAdditionalProperties(builder);
        }

        protected abstract void ConfigureAdditionalProperties(EntityTypeBuilder<TEntity> builder);
    }
}
