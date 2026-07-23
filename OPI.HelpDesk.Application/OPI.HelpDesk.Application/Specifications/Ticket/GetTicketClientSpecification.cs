using OPI.HelpDesk.Application.Specifications;
using OPI.HelpDessk.Domain.Entities;

namespace OPI.HelpDesk.Application.Specifications.Tickets
{
    public class GetTicketClientSpecification : BaseSpecification<User>
    {
        public GetTicketClientSpecification(Guid id)
        {
            AddCriteria(user => user.Id == id);
        }
    }
}
