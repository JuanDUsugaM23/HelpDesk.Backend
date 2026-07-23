using OPI.HelpDesk.Application.Specifications;
using OPI.HelpDessk.Domain.Entities;

namespace OPI.HelpDesk.Application.Specifications.Tickets
{
    public class GetTicketByIdSpecification : BaseSpecification<Ticket>
    {
        public GetTicketByIdSpecification(Guid id)
        {
            AddCriteria(t => t.Id == id);

            AddInclude(t => t.Client);
            AddInclude(t => t.AssignedTechnical);
        }
    }
}