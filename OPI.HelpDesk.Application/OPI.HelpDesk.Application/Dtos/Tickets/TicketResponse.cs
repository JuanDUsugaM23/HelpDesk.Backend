using OPI.HelpDessk.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPI.HelpDesk.Application.DTOs.Ticket
{
    public class TicketResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public Guid ClientId { get; set; }
        public string ClientName { get; set; }
        public Guid? AssignedTechnicianId { get; set; }
        public string? AssignedTechnicianName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsOverdue { get; set; }
    }
}
