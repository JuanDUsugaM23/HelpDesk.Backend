using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPI.HelpDessk.Domain.Entities;

namespace OPI.HelpDesk.Infrastructure.Persistence.Configurations
{
    public class RefreshTokenConfiguration
    {
        public RefreshTokenConfiguration(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshToken");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Token)
                .IsRequired()
                .HasMaxLength(500);
            builder.HasIndex(t => t.Token).IsUnique();
            builder.Property(t => t.ExpiresAt)
                .IsRequired()
                .HasColumnType("timestamp with time zone");
            builder.HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(t => new { t.UserId, t.IsRevoked });
            builder.Property(t => t.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(t => t.UpdatedAt)
                .HasColumnType("timestamp with time zone");
            builder.Property(t => t.Enable)
                .IsRequired()
                .HasDefaultValue(true);
            builder.Property(t => t.IsRevoked)
                .HasDefaultValue(false);
        }
    }
}
