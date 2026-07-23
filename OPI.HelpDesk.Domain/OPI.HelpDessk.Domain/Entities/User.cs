using OPI.HelpDessk.Domain.Entities.Enums;

namespace OPI.HelpDessk.Domain.Entities
{
    public class User : AuidEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? HashPassword { get; set; }
        public RolesEnum Role { get; set; } = RolesEnum.Client;
        public List<TicketCategoriesEnum> Specialities { get; set; } = new List<TicketCategoriesEnum>();
        public int MaxTicket { get; set; } = 3;

        public IEnumerable<Ticket> AssignedTickets { get; set; } = new List<Ticket>();
        public IEnumerable<Ticket> CreateTickets { get; set; } = new List<Ticket>();
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();

    }
}
