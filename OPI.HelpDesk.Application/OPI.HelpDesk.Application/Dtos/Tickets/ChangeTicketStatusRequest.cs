using OPI.HelpDessk.Domain.Entities.Enums;

namespace OPI.HelpDesk.Application.DTOs.Ticket
{
    public class ChangeTicketStatusRequest
    {
        public TicketStatusEnum Status { get; set; }
        public string? Note { get; set; }
    }
}
