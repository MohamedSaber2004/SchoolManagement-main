using School.Domain.Entities;

namespace School.Application.Interfaces.Repositories
{
    public interface IChatGenericRepository<TEntity,TKey>: IGenericRepository<TEntity,TKey> where TEntity : BaseEntity<TKey>
    {
    }
}
