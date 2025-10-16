using School.Application.Interfaces.Repositories;
using School.Domain.Entities;
using School.Infrastructure.Data;

namespace School.Infrastructure.Implementation.Repositories
{
    internal class UnitOfWork(SchoolDbContext _dbContext) : IUnitOfWork
    {
        private readonly Dictionary<string, object> _repositories = [];

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var typeName = typeof(TEntity).Name;
            if(_repositories.ContainsKey(typeName))
                return (IGenericRepository<TEntity, TKey>)_repositories[typeName];
            else
            {
                var repositoryInstance = new GenericRepository<TEntity, TKey>(_dbContext);
                _repositories.Add(typeName, repositoryInstance);
                return repositoryInstance;
            }
        }

        public async Task<int> SaveChangesAsync() 
            => await _dbContext.SaveChangesAsync();
    }
}
