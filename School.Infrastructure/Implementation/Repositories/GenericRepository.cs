using Microsoft.EntityFrameworkCore;
using School.Application.Interfaces;
using School.Application.Interfaces.Repositories;
using School.Domain.Entities;
using School.Infrastructure.Data;
using School.Infrastructure.Extensions;

namespace School.Infrastructure.Implementation.Repositories
{
    public class GenericRepository<TEntity, TKey>(SchoolDbContext _dbContext) : IGenericRepository<TEntity, TKey> where TEntity: BaseEntity<TKey>
    {
        public async Task AddAsync(TEntity entity) 
            => await _dbContext.Set<TEntity>().AddAsync(entity);

        public async Task<int> CountAsync(ISpecifications<TEntity, TKey> spec)
            => await SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), spec).CountAsync();

        public void Delete(TEntity entity) 
            => _dbContext.Set<TEntity>().Remove(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync() 
            => await _dbContext.Set<TEntity>().ToListAsync();

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity, TKey> spec)
            => await SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), spec).ToListAsync();

        public async Task<TEntity?> GetByIdAsync(TKey id) 
            => await _dbContext.Set<TEntity>().FindAsync(id);

        public async Task<TEntity?> GetByIdAsync(ISpecifications<TEntity, TKey> spec)
            => await SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), spec).FirstOrDefaultAsync(); 

        public void Update(TEntity entity)
           => _dbContext.Set<TEntity>().Update(entity);

        public void UpdateRange(IEnumerable<TEntity> entities) 
            => _dbContext.Set<TEntity>().UpdateRange(entities);
    }
}
