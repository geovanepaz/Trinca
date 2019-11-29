using Core.Entities.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Mappings
{
    public class LogMap : IEntityTypeConfiguration<Log>, IMappingTrincaLog
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.ToTable("log_sistema_churrasco").HasKey(o => o.Id);

            builder.Property(o => o.Id).HasColumnName("id").HasColumnType("int").IsRequired();
            builder.Property(o => o.Application).HasColumnName("application").HasColumnType("varchar(255)").IsRequired();
            builder.Property(o => o.Tracetime).HasColumnName("tracetime").HasColumnType("datetime").IsRequired();
            builder.Property(o => o.Loglevel).HasColumnName("loglevel").HasColumnType("varchar(50)").IsRequired();
            builder.Property(o => o.Message).HasColumnName("message").HasColumnType("varchar(4000)").IsRequired();
            builder.Property(o => o.Machinename).HasColumnName("machinename").HasColumnType("varchar(20)").IsRequired();
            builder.Property(o => o.Username).HasColumnName("username").HasColumnType("varchar(30)").IsRequired();
            builder.Property(o => o.ExceptionMessage).HasColumnName("exceptionmessage").HasColumnType("varchar(4000)").IsRequired();
        }
    }
}