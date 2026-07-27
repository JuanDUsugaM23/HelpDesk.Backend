using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPI.HelpDessk.Domain.Entities;

namespace OPI.HelpDesk.Infrastructure.Persistence.Configurations
{
    public class TicketHistoryLogConfiguration
    {
        public TicketHistoryLogConfiguration(EntityTypeBuilder<TicketHistoryLog> builder)
        {
            builder.ToTable("TicketHistortyLog");
            builder.HasKey(th => th.Id);
            builder.HasOne(th => th.Ticket)
                .WithMany(t => t.History)
                .HasForeignKey(th => th.TicketId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(th => th.FromStatus)
                .IsRequired();
            builder.Property(th => th.ToStatus)
                .IsRequired();
            builder.Property(th => th.Note)
                .HasMaxLength(500);
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

