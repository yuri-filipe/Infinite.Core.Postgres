namespace Infinite.Core.Postgres.Mapping
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    public abstract class CoreMapping<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
     where TEntity : CoreEntity<TKey>
    {
        protected abstract string Schema { get; }
        protected abstract string TableName { get; }

        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.ToTable(TableName, Schema);

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd()
                .HasColumnOrder(0);

            builder.Property(e => e.Excluido)
                .HasColumnName("EXCLUIDO")
                .HasDefaultValue(false)
                .HasColumnOrder(1);

            builder.Property(e => e.UsuarioInclusao)
                .HasColumnName("USUARIO_INCLUSAO")
                .HasMaxLength(100)
                .HasColumnOrder(2);

            builder.Property(e => e.DataInclusao)
                .HasColumnName("DATA_INCLUSAO")
                .HasColumnOrder(3);

            builder.Property(e => e.UsuarioAlteracao)
                .HasColumnName("USUARIO_ALTERACAO")
                .HasMaxLength(100)
                .HasColumnOrder(4);

            builder.Property(e => e.DataAlteracao)
                .HasColumnName("DATA_ALTERACAO")
                .HasColumnOrder(5);

            ConfigureAdditionalProperties(builder);
        }
        protected abstract void ConfigureAdditionalProperties(EntityTypeBuilder<TEntity> builder);
    }
}
