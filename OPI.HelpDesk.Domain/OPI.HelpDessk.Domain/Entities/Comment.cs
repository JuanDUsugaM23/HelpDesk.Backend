namespace OPI.HelpDessk.Domain.Entities
{
    public class Comment :AuidEntity
    {
        public Guid TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
        public string? Text { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
