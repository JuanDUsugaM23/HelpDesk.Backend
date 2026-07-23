using OPI.HelpDessk.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPI.HelpDesk.Application.DTOs.Ticket
{
    public class CreateTicketRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketCategoriesEnum Category { get; set; } 
        public TicketPriorityEnum Priority { get; set; }
        public Guid ClientId { get; set; }
    }
}
