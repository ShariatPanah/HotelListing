using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotelListing.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        Task AddAsync(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        void Attach(TEntity entity);
        void Detach(TEntity entity);
        IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties);

        void Remove(params object[] keyValues);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        TEntity Find(params object[] keyValues);
        Task<TEntity> FindAsync(params object[] keyValues);
        IEnumerable<TEntity> GetAll();
        IQueryable<TEntity> GetAllAsQueryable();

        TEntity FirstOrDefault();
        Task<TEntity> FirstOrDefaultAsync();
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        int Count(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        bool Any(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        bool Any();
        Task<bool> AnyAsync();
        IQueryable<TEntity> FromSqlInterpolated(FormattableString sqlQuery);
        IQueryable<TEntity> FromSqlRaw(string sqlQuery, params object[] parameters);
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
        //IQueryable<TEntity> OrderBy<TEntity, TKey>(Expression<Func<TEntity, TKey>> keySelector) where TEntity : class;
        //IOrderedQueryable<TEntity> OrderBy<TEntity, TKey>(Expression<Func<TEntity, TKey>> keySelector);
        IOrderedQueryable<TEntity> OrderByDescending(Expression<Func<TEntity, object>> keySelector);
        void MarkAsModified(TEntity entity);
        //DbEntityEntry<TEntity> Entry(TEntity entity);
    }
}
