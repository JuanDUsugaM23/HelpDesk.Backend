using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPI.HelpDessk.Domain.Entities;

namespace OPI.HelpDesk.Infrastructure.Persistence.Configurations
{
    public class SettingSLAConfiguration
    {
        public SettingSLAConfiguration(EntityTypeBuilder<SettingSLA> builder)
        {
            builder.ToTable("SettingsSLA");
            builder.HasKey(s => s.Id);
            builder.Property(t => t.Priority)
                .HasConversion<int>()
                .IsRequired();
            builder.Property(t => t.Categority)
                .HasConversion<int>()
                .IsRequired();
            builder.Property(t => t.LimitTime)
                .HasPrecision(18, 2) 
                .IsRequired()
                .HasDefaultValue(0.0);
            builder.Property(a => a.CreatedAt)
                    .IsRequired()
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(a => a.CreatedBy)
                .IsRequired();
            builder.Property(a => a.UpdatedAt)
                .HasColumnType("timestamp with time zone");
            builder.Property(a => a.Enable)
                .IsRequired()
                .HasDefaultValue(true);
        }
    }
}
