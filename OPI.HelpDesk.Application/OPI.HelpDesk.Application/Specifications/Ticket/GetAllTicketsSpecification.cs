using OPI.HelpDesk.Application.Specifications;
using OPI.HelpDessk.Domain.Entities;

namespace OPI.HelpDesk.Application.Specifications.Tickets
{
    public class GetAllTicketsSpecification : BaseSpecification<Ticket>
    {
        public GetAllTicketsSpecification()
        {
            AddInclude(t => t.Client);
            AddInclude(t => t.AssignedTechnical);

            ApplyOrderByDescending(t => t.CreatedAt);
        }
    }
}