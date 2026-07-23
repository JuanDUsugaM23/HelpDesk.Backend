using OPI.HelpDesk.Application.DTOs.Ticket;
using OPI.HelpDessk.Domain.Entities;

namespace OPI.HelpDesk.Application.Mappings
{
    public static class TicketMapper
    {
        public static TicketResponse ToResponse(Ticket ticket)
        {
            return new TicketResponse
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                Category = ticket.Category,
                Priority = ticket.Priority,
                Status = ticket.Status,
                ClientId = ticket.CreatedBy,
                AssignedTechnicalId = ticket.AssignedTechnicalId,
                CreatedAt = ticket.CreatedAt,
                IsOverdue = ticket.IsOverdue
            };
        }
    }
}