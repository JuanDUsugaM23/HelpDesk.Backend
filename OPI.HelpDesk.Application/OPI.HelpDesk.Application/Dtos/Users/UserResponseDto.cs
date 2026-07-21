using OPI.HelpDessk.Domain.Entities;

namespace OPI.HelpDesk.Application.Dtos.Users
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public List<string> Specialities { get; set; }
        public int MaxTicket { get; set; }
        public IEnumerable<Ticket> AssignedTickets { get; set; } = new List<Ticket>();
        public IEnumerable<Ticket> CreateTickets { get; set; } = new List<Ticket>();
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
        public bool Enable { get; set; }
    }
}
