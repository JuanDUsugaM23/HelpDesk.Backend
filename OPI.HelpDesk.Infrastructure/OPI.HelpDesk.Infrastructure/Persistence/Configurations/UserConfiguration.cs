using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPI.HelpDessk.Domain.Entities;

namespace OPI.HelpDesk.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration
    {
        public UserConfiguration(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(180);
            builder.Property(u => u.HashPassword)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(u => u.Role)
                .HasConversion<int>()
                .IsRequired();
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
