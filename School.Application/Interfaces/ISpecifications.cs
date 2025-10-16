using School.Domain.Entities;
using System.Linq.Expressions;

namespace School.Application.Interfaces
{
    public interface ISpecifications<TEntity,TKey> where TEntity : BaseEntity<TKey>
    {
        Expression<Func<TEntity, bool>>? Criteria { get; }
        Expression<Func<TEntity, object>> OrderBy { get; }
        Expression<Func<TEntity, object>> OrderByDesc { get; }
        List<Expression<Func<TEntity, object>>> IncludeExpressions { get; }

        public int Take { get; }
        public int Skip { get; }
        public bool IsPaginated { get; set; }
    }
}
