using OPI.HelpDesk.Application.Dtos.Tickets;
using OPI.HelpDesk.Application.DTOs.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPI.HelpDesk.Application.Interfaces.Ticket
{
    public interface ITicketService
    {
        Task<TicketResponse> CreateAsync(CreateTicketRequest request);
        Task<IEnumerable<TicketResponse>> GetAllAsync(GetAllTicketsQueryDto query);
        Task<TicketResponse?> GetByIdAsync(Guid id);
        Task<TicketResponse?> UpdateAsync(Guid id,UpdateTicketRequest request);
        Task<TicketResponse?> AssignAsync(Guid id, AssignTicketRequest request);
        Task<TicketResponse?> ChangeStatusAsync(Guid id, ChangeTicketStatusRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}
