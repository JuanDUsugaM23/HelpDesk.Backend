using Microsoft.EntityFrameworkCore;
using OPI.HelpDesk.Application.Interfaces.Auditoires;
using OPI.HelpDesk.Infrastructure.Persistence.Configurations;
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ModelConfig(modelBuilder);
        }

        private void ModelConfig(ModelBuilder model)
        {
            new CommentConfiguration(model.Entity<Comment>());
            new RefreshTokenConfiguration(model.Entity<RefreshToken>());
            new SettingSLAConfiguration(model.Entity<SettingSLA>());
            new TicketConfiguration(model.Entity<Ticket>());
            new TicketHistoryLogConfiguration(model.Entity<TicketHistoryLog>());
            new UserConfiguration(model.Entity<User>());
        }
    }
}
