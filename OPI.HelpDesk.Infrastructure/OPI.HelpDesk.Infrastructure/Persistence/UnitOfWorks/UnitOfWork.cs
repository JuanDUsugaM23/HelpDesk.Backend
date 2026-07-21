using OPI.HelpDesk.Application.Interfaces.Repositories;
using OPI.HelpDesk.Application.Interfaces.UnitOfWorks;
using OPI.HelpDesk.Infrastructure.Persistence.Repositories;
using OPI.HelpDessk.Domain.Entities;
using System.Collections;

namespace OPI.HelpDesk.Infrastructure.Persistence.UnitOfWorks
{
    public class UnitOfWork(HelpDeskDbContext context) : IUnitOfWork
    {
        private readonly Dictionary<string, object> repositories = new();
        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }

        public IGenericRepository<T> Repository<T>() where T : AuidEntity
        {
            var type = typeof(T).Name;
            if (!repositories.ContainsKey(type))
            {
                GenericRepository<T> repository = new GenericRepository<T>(context);
                repositories.Add(type, repository);
            }
            return (IGenericRepository<T>)repositories[type]!;
        }

        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}
