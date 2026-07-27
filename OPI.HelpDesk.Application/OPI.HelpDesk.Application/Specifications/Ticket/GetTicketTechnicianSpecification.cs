using OPI.HelpDesk.Application.Specifications;
using OPI.HelpDessk.Domain.Entities;
using OPI.HelpDessk.Domain.Entities.Enums;

namespace OPI.HelpDesk.Application.Specifications.Tickets
{
    public class GetTicketTechnicianSpecification : BaseSpecification<User>
    {
        public GetTicketTechnicianSpecification(Guid id)
        {
            AddCriteria(user => user.Id == id && user.Enable && user.Role == RolesEnum.Technical);
        }
    }
}
