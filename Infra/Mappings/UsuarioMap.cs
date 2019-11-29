using Core.Entities.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Mappings
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>, IMappingTrinca
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("usuario").HasKey(o => o.Id);

            builder.Property(o => o.Id).HasColumnName("id").HasColumnType("int").IsRequired();
            builder.Property(o => o.NomeCompleto).HasColumnName("nome_completo").HasColumnType("varchar(255)").IsRequired();
            builder.Property(o => o.Email).HasColumnName("email").HasColumnType("varchar(255)").IsRequired();
            builder.Property(o => o.Senha).HasColumnName("senha").HasColumnType("varchar(255)").IsRequired();
        }
    }
}