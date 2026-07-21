using OPI.HelpDessk.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPI.HelpDesk.Application.Dtos.Users
{
    public class UserAddRequestDto
    {
        public string FristName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public RolesEnum Role { get; set; }
        public List<TicketCategoriesEnum>? Specialities { get; set; } = new List<TicketCategoriesEnum>();
        public int? MaxTicket { get; set; }
    }
}
