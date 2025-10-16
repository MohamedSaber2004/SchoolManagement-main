using School.Domain.Entities;

namespace School.Application.Interfaces.Repositories
{
    public interface IChatUnitOfWork
    {
        public IChatGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;

        Task<int> SaveChangesAsync();
    }
}
