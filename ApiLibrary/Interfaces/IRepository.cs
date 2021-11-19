using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApiLibrary.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {

        Task<TEntity> Get(int id);
        IEnumerable<TEntity> GetAll();
        void Add(TEntity entity);
        Task AddAsync(TEntity entity);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
        void Remove(TEntity entity);

    }
}
