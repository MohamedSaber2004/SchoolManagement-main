using School.Application.Common.Enums;
using School.Application.Interfaces;
using School.Domain.Entities;
using System.Linq.Expressions;

namespace School.Infrastructure.Implementation.Specifications
{
    public class BaseSpecification<TEntity, TKey> : ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        protected BaseSpecification(Expression<Func<TEntity,bool>>? expression)
        {
            Criteria = expression;
            OrderBy = null!;
            OrderByDesc = null!;
        }

        public Expression<Func<TEntity, bool>>? Criteria { get; private set; }

        public Expression<Func<TEntity, object>> OrderBy { get; private set; }

        public Expression<Func<TEntity, object>> OrderByDesc {  get; private set; }

        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPaginated { get; set; }

        protected void AddOrderBy(Expression<Func<TEntity,object>> orderBy)
            => OrderBy = orderBy;

        protected void AddOrderByDesc(Expression<Func<TEntity,object>> orderByDesc)
            => OrderByDesc = orderByDesc;

        protected void AddInclude(Expression<Func<TEntity,object>> include)
            => IncludeExpressions.Add(include);

        protected void ApplyPagination(int pageSize, int pageNumber)
        {
            IsPaginated = true;
            Take = pageSize;
            Skip = (pageNumber - 1) * pageSize;
        }
    }
}
