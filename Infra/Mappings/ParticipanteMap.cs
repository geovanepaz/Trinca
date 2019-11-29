using Core.Entities.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Mappings
{
    public class ParticipanteMap : IEntityTypeConfiguration<Participante>, IMappingTrinca
    {
        public void Configure(EntityTypeBuilder<Participante> builder)
        {
            builder.ToTable("participante").HasKey(o => o.Id);

            builder.Property(o => o.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(o => o.NomeCompleto).HasColumnName("nome_completo").HasColumnType("varchar(255)");
            builder.Property(o => o.Apelido).HasColumnName("apelido").HasColumnType("varchar(255)").IsRequired();
            builder.Property(o => o.Email).HasColumnName("email").HasColumnType("varchar(255)").IsRequired();
            
        }
    }
}