using Microsoft.EntityFrameworkCore;
using OPI.HelpDesk.Application.Interfaces.Auditoires;
using OPI.HelpDessk.Domain.Entities;

namespace OPI.HelpDesk.Infrastructure.Persistence
{
    public class HelpDeskDbContext(DbContextOptions options, ICurrentUserService currentUserService) : DbContext(options)
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<SettingSLA> SettingsSlas { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketHistoryLog> TicketHistoryLogs { get; set; }
        public DbSet<User> Users { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            Guid userId = currentUserService.UserId ?? Guid.Empty;
            DateTime now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<AuidEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = now;
                        entry.Entity.Enable = true;
                        if (entry.Entity.CreatedBy == Guid.Empty || entry.Entity.CreatedBy == null)
                            entry.Entity.CreatedBy = userId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = now;
                        entry.Entity.UpdatedBy = userId;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
