using OPI.HelpDesk.Application.Dtos.Comments;
using OPI.HelpDessk.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPI.HelpDesk.Application.Specifications.Comments
{
    public class CommentSpecificationFilter : BaseSpecification<Comment>
    {
        public CommentSpecificationFilter(GetAllCommentsQueryDto query)
        {
            if (query.IsEnable.HasValue)
                AddCriteria(c => c.Enable == query.IsEnable.Value);
            if (query.Id.HasValue)
                AddCriteria(c => c.Id == query.Id.Value);
            if (query.TicketId.HasValue)
                AddCriteria(c => c.TicketId == query.TicketId.Value);

            AddInclude(c => c.Ticket);

            if (query.IsDescending)
                ApplyOrderByDescending(c => c.CreatedAt);
            else
                ApplyOrderBy(c => c.CreatedAt);

            ApplyPaging((query.Page - 1) * query.PageSize, query.PageSize);
        }
    }
}
