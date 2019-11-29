using Core.Entities.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Mappings
{
    public class EventoMap : IEntityTypeConfiguration<Evento>, IMappingTrinca
    {
        public void Configure(EntityTypeBuilder<Evento> builder)
        {
            builder.ToTable("evento").HasKey(o => o.Id);

            builder.Property(o => o.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(o => o.Descricao).HasColumnName("descricao").HasColumnType("varchar(255)");
            builder.Property(o => o.Observacao).HasColumnName("observacao").HasColumnType("varchar(500)");
            builder.Property(o => o.DataEvento).HasColumnName("data_evento").HasColumnType("date").IsRequired();
        }
    }
}