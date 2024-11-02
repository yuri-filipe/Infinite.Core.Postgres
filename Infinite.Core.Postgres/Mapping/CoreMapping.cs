namespace Infinite.Core.Postgres.Mapping
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    public abstract class CoreMapping<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : CoreEntity
    {
        protected abstract string Schema { get; }
        protected abstract string TableName { get; }

        private readonly string[] fixedProperties =
        {
            nameof(CoreEntity.UsuarioAlteracao),
            nameof(CoreEntity.DataAlteracao),
            nameof(CoreEntity.UsuarioInclusao),
            nameof(CoreEntity.DataInclusao),
            nameof(CoreEntity.Excluido)
        };

        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.ToTable(TableName, Schema);

            builder.HasKey(e => e.Id);

            // Configuração adicional específica de cada entidade
            ConfigureAdditionalProperties(builder);

            // Configuração dinâmica das propriedades, exceto as fixas
            var allProperties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            int order = 1;

            foreach (var property in allProperties)
            {
                // Pula as propriedades fixas
                if (fixedProperties.Contains(property.Name)) continue;

                builder.Property(property.Name)
                    .HasColumnName(ConvertToColumnFormat(property.Name))
                    .HasColumnOrder(order++);
            }

            // Configurando as colunas fixas no final
            ConfigureFixedProperties(builder, ref order);
        }

        private void ConfigureFixedProperties(EntityTypeBuilder<TEntity> builder, ref int order)
        {
            builder.Property(e => e.UsuarioAlteracao)
                .HasColumnName("USUARIO_ALTERACAO")
                .IsRequired(true)
                .HasDefaultValue("-")
                .HasMaxLength(100)
                .HasColumnOrder(order++);

            builder.Property(e => e.DataAlteracao)
                .HasColumnName("DATA_ALTERACAO")
                .IsRequired(true)
                .HasColumnOrder(order++);

            builder.Property(e => e.UsuarioInclusao)
                .HasColumnName("USUARIO_INCLUSAO")
                .HasMaxLength(100)
                .IsRequired(true)
                .HasDefaultValue("-")
                .HasColumnOrder(order++);

            builder.Property(e => e.DataInclusao)
                .HasColumnName("DATA_INCLUSAO")
                .IsRequired(true)
                .HasColumnOrder(order++);

            builder.Property(e => e.Excluido)
                .HasColumnName("EXCLUIDO")
                .IsRequired(true)
                .HasDefaultValue(false)
                .HasColumnOrder(order++);
        }

        protected abstract void ConfigureAdditionalProperties(EntityTypeBuilder<TEntity> builder);

        private string ConvertToColumnFormat(string propertyName)
        {
            // Usa regex para adicionar o padrão TESTE_TESTE baseado no nome da propriedade
            var formattedName = Regex.Replace(propertyName, "([a-z])([A-Z])", "$1_$2").ToUpper();
            return formattedName;
        }
    }
}