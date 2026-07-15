namespace OPI.HelpDessk.Domain.Entities
{
    public class TicketHistoryLog : AuidEntity
    {
        public Guid TicketId { get; set; }
        public string? FromStatus { get; set; }
        public string? ToStatus { get; set; }
        public string? Note { get; set; }
    }
}
