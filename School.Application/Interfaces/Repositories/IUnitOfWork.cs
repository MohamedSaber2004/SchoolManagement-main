using School.Domain.Entities;

namespace School.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        public IGenericRepository<TEntity,TKey> GetRepository<TEntity, TKey>() where TEntity:BaseEntity<TKey>;

        Task<int> SaveChangesAsync();
    }
}
