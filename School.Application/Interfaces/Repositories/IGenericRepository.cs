using School.Domain.Entities;

namespace School.Application.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity,TKey> where TEntity: BaseEntity<TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity,TKey> spec);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(ISpecifications<TEntity,TKey> spec);
        Task<TEntity?> GetByIdAsync(TKey id);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);

        Task<int> CountAsync(ISpecifications<TEntity,TKey> spec);
    }
}
