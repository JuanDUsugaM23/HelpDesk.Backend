using OPI.HelpDessk.Domain.Entities;
using System.Linq.Expressions;

namespace OPI.HelpDesk.Application.Specifications
{
    public class SpecificationBuilder<T> : BaseSpecification<T> where T : AuidEntity
    {
        public SpecificationBuilder<T> Where(Expression<Func<T, bool>> predicate)
        {
            AddCriteria(predicate);
            return this;
        }

        public SpecificationBuilder<T> Include(Expression<Func<T, object>> include)
        {
            AddInclude(include);
            return this;
        }

        public SpecificationBuilder<T> Page(int page, int size)
        {
            ApplyPaging((page - 1) * size, size);
            return this;
        }
    }
}
