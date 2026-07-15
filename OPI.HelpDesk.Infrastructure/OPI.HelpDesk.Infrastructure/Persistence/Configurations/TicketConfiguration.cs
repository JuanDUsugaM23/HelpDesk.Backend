using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPI.HelpDessk.Domain.Entities;

namespace OPI.HelpDesk.Infrastructure.Persistence.Configurations
{
    public class TicketConfiguration
    {
        public TicketConfiguration(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("Ticket");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(t => t.Description) 
                .IsRequired()
                .HasMaxLength(1000);
            builder.Property(t => t.Category)
                .HasConversion<int>()
                .IsRequired();
            builder.Property(t => t.Priority)
                .HasConversion<int>()
                .IsRequired();
            builder.Property(t => t.Status)
                .HasConversion<int>()
                .IsRequired();
            builder.HasOne(t => t.Client)
                .WithMany(u => u.CreateTickets)
                .HasForeignKey(t => t.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.AssignedTechnical)
                .WithMany(u => u.AssignedTickets)
                .HasForeignKey(t => t.AssignedTechnicalId)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Property(t => t.SlaLimitTime)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(t => t.ResolvedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(t => t.ClosedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
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
