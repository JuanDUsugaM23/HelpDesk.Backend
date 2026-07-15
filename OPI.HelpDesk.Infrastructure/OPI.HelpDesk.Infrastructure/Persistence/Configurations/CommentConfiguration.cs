using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPI.HelpDessk.Domain.Entities;

namespace OPI.HelpDesk.Infrastructure.Persistence.Configurations
{
    public class CommentConfiguration
    {
        public CommentConfiguration(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comment");
            builder.HasKey(c => c.Id);
            builder.HasOne(c => c.Ticket)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TicketId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(c => c.Text)
                .HasMaxLength(500);
        builder.HasOne(c => c.User)
                .WithMany()
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
    }
}
