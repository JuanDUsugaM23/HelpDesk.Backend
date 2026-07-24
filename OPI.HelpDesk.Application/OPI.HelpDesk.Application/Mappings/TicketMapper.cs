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
                Category = ticket.Category.ToString(),
                Priority = ticket.Priority.ToString(),
                Status = ticket.Status.ToString(),
                ClientId = ticket.CreatedBy,
                ClientName = $"{ticket.Client.FirstName} {ticket.Client.LastName}",
                AssignedTechnicianId = ticket.AssignedTechnical?.Id,
                AssignedTechnicianName = ticket.AssignedTechnical == null
                ? null: $"{ticket.AssignedTechnical.FirstName} {ticket.AssignedTechnical.LastName}",
                CreatedAt = ticket.CreatedAt,
                IsOverdue = ticket.IsOverdue
            };
        }
    }
}