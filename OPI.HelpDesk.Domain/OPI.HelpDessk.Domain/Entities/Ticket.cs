using OPI.HelpDessk.Domain.Entities.Enums;

namespace OPI.HelpDessk.Domain.Entities
{
    public class Ticket : AuidEntity
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TicketCategoriesEnum Category { get; set; }
        public TicketPriorityEnum Priority { get; set; }
        public TicketStatusEnum Status { get; set; }
        public Guid ClientId { get; set; }
        public virtual User Client { get; set; }
        public Guid AssignedTechnicalId { get; set; }
        public virtual User AssignedTechnical { get; set; }
        public DateTime SlaLimitTime { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public bool IsOverdue => DateTime.UtcNow > SlaLimitTime && ResolvedAt == null;
        public IEnumerable<TicketHistoryLog> History { get; set; } = new List<TicketHistoryLog>();
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();

    }
}
