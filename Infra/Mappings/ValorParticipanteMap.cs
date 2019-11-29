using Core.Entities.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Mappings
{
    public class ValorParticipanteMap : IEntityTypeConfiguration<ValorParticipante>, IMappingTrinca
    {
        public void Configure(EntityTypeBuilder<ValorParticipante> builder)
        {
            builder.ToTable("valor_participante").HasKey(o => o.Id);

            builder.Property(o => o.Id).HasColumnName("id").HasColumnType("int").IsRequired();
            builder.Property(o => o.IdEvento).HasColumnName("id_evento").HasColumnType("int").IsRequired();
            builder.Property(o => o.IdParticipante).HasColumnName("id_participante").HasColumnType("int").IsRequired();
            builder.Property(o => o.ValorSugerido).HasColumnName("valor_sugerido").HasColumnType("decimal(4,2)");
            builder.Property(o => o.Valor).HasColumnName("valor").HasColumnType("decimal(4,2)");
            
        }
    }
}