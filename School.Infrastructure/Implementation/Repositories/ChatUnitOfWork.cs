using School.Application.Interfaces.Repositories;
using School.Domain.Entities;
using School.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Infrastructure.Implementation.Repositories
{
    public class ChatUnitOfWork(ChatDbContext _dbContext) : IChatUnitOfWork
    {
        private readonly Dictionary<string, object> _repositories = new();
        public IChatGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var typeName = typeof(TEntity).Name;
            if(_repositories.ContainsKey(typeName))
                return (IChatGenericRepository<TEntity, TKey>)_repositories[typeName];
            else
            {
                var repositoryInstance = new ChatGenericRepository<TEntity, TKey>(_dbContext);
                _repositories.Add(typeName, repositoryInstance);
                return repositoryInstance;
            }
        }

        public async Task<int> SaveChangesAsync() 
            => await _dbContext.SaveChangesAsync();
    }
}
