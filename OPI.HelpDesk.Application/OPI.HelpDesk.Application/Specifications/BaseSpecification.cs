using OPI.HelpDesk.Application.Interfaces.Specifications;
using OPI.HelpDessk.Domain.Entities;
using System.Linq.Expressions;

namespace OPI.HelpDesk.Application.Specifications
{
    public class BaseSpecification<T> : ISpecification<T> where T : AuidEntity
    {
        private List<Expression<Func<T, bool>>>? _criteria { get; } = [];
        public Expression<Func<T, bool>> Criteria => _criteria.Count == 0 ? null : _criteria.Aggregate(AndAlso);
        public List<Expression<Func<T, object>>>? Includes { get; } = [];
        public Expression<Func<T, object>>? OrderBy { get; protected set; }
        public Expression<Func<T, object>>? OrderByDescending { get; protected set; }
        public int? Skip { get; protected set; }
        public int? Take { get; protected set; }

        protected void AddCriteria(Expression<Func<T, bool>> criteria) => _criteria.Add(criteria);
        protected void AddInclude(Expression<Func<T, object>> includeExpression) => Includes?.Add(includeExpression);
        protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
            => OrderBy = orderByExpression;

        protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
            => OrderByDescending = orderByDescExpression;

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
        }

        private static Expression<Func<T, bool>> AndAlso(
            Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            ParameterExpression param = Expression.Parameter(typeof(T));

            BinaryExpression body = Expression.AndAlso(
                Expression.Invoke(left, param),
                Expression.Invoke(right, param));

            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }
}
