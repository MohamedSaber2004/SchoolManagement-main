using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using School.Application.Interfaces;
using School.Application.Interfaces.Repositories;
using School.Domain.Entities;

namespace School.Infrastructure.Extensions
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity,TKey>(IQueryable<TEntity> inputQuery, ISpecifications<TEntity,TKey> specifications) where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery;

            if(specifications.Criteria is not null)
            {
                query = query.Where(specifications.Criteria);
            }

            if(specifications.OrderBy is not null)
            {
                query = query.OrderBy(specifications.OrderBy);
            }

            if(specifications.OrderByDesc is not null)
            {
                query = query.OrderByDescending(specifications.OrderByDesc);
            }

            if(specifications.IncludeExpressions is not null && specifications.IncludeExpressions.Count > 0)
            {
                query = specifications.IncludeExpressions.Aggregate(query, (current, include) => current.Include(include));
            }

            if(specifications.IsPaginated)
            {
                query = query.Skip(specifications.Skip).Take(specifications.Take);
            }

            return query;
        }   
    }
}
