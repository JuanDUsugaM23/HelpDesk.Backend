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
        public TicketCategoriesEnum Category { get; set; }
        public TicketPriorityEnum Priority { get; set; }
        public TicketStatusEnum Status { get; set; }
        public Guid ClientId { get; set; }
        public Guid? AssignedTechnicalId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsOverdue { get; set; }
    }
}
