using Microsoft.EntityFrameworkCore;
using OPI.HelpDesk.Application.Interfaces.Repositories;
using OPI.HelpDesk.Application.Interfaces.Specifications;
using OPI.HelpDesk.Infrastructure.Specifications;
using OPI.HelpDessk.Domain.Entities;

namespace OPI.HelpDesk.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T>(HelpDeskDbContext context) : IGenericRepository<T> where T : AuidEntity
    {
        protected readonly DbSet<T> _dbSet = context.Set<T>();

        public async Task AddAsync(T entity, CancellationToken ct = default)
            => await _dbSet.AddAsync(entity, ct);

        public async Task<int> CountAsync(CancellationToken ct = default)
            => await _dbSet.CountAsync(ct);

        public void Delete(T entity)
         => _dbSet.Remove(entity);

        public async Task<IEnumerable<T>> GetAllAsync(ISpecification<T> spec, CancellationToken ct = default)
        {
            IQueryable<T> query = SpecificationEvaluator<T>.GetQuery(_dbSet.AsQueryable(), spec);
            return await query.ToListAsync(ct);
        }

        public async Task<T?> GetAsync(ISpecification<T> spec, CancellationToken ct = default)
        {
            IQueryable<T> query = SpecificationEvaluator<T>.GetQuery(_dbSet.AsQueryable(), spec);
            return await query.FirstOrDefaultAsync(ct);
        }

        public void Update(T entity)
         => _dbSet.Update(entity);

       
    }
}
