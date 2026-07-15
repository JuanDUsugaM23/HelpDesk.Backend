using OPI.HelpDesk.Application.Interfaces.Specifications;
using OPI.HelpDessk.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace OPI.HelpDesk.Infrastructure.Specifications
{
    public class SpecificationEvaluator<T> where T : AuidEntity
    {
        public static IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T> spec) where T : AuidEntity
        {
            IQueryable<T> query = inputQuery;

            if (spec.Criteria is not null)
                query = query.Where(spec.Criteria);

            if (spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);
            else if (spec.OrderByDescending is not null)
                query = query.OrderByDescending(spec.OrderByDescending);

            if (spec.Skip.HasValue)
                query = query.Skip(spec.Skip.Value);

            if (spec.Take.HasValue)
                query = query.Take(spec.Take.Value);

            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
            return query;
        }
    }
}
