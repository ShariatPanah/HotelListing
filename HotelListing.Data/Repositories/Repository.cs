using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HotelListing.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbset;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbset = _context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            _dbset.Add(entity);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbset.AddAsync(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _dbset.AddRange(entities);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbset.AddRangeAsync(entities);
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbset.Any(predicate);
        }

        public bool Any()
        {
            return _dbset.Any();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbset.AnyAsync(predicate);
        }

        public async Task<bool> AnyAsync()
        {
            return await _dbset.AnyAsync();
        }

        public void Attach(TEntity entity)
        {
            if (entity == null)
                throw new NullReferenceException();

            if (_context.Entry(entity).State == EntityState.Detached)
                _dbset.Attach(entity);
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbset.Count(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbset.CountAsync(predicate);
        }

        public void Detach(TEntity entity)
        {
            var entry = _context.Entry(entity);
            if (entry != null)
                entry.State = EntityState.Detached;
        }

        public TEntity Find(params object[] keyValues)
        {
            return _dbset.Find(keyValues);
        }

        public async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await _dbset.FindAsync(keyValues);
        }

        public TEntity FirstOrDefault()
        {
            return _dbset.FirstOrDefault();
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbset.FirstOrDefault(predicate);
        }

        public async Task<TEntity> FirstOrDefaultAsync()
        {
            return await _dbset.FirstOrDefaultAsync();
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbset.FirstOrDefaultAsync(predicate);
        }

        public IQueryable<TEntity> FromSqlInterpolated(FormattableString sqlQuery)
        {
            return _dbset.FromSqlInterpolated(sqlQuery);
        }

        public IQueryable<TEntity> FromSqlRaw(string sqlQuery, params object[] parameters)
        {
            return _dbset.FromSqlRaw(sqlQuery, parameters);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbset.AsEnumerable();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbset.ToListAsync();
        }

        public IQueryable<TEntity> GetAllAsQueryable()
        {
            return _dbset.AsQueryable();
        }

        public IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbset;

            foreach (var property in includeProperties)
            {
                query = query.Include(property);
            }

            return query;
        }

        public void MarkAsModified(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public IOrderedQueryable<TEntity> OrderByDescending(Expression<Func<TEntity, object>> keySelector)
        {
            return _dbset.OrderByDescending(keySelector);
        }

        public void Remove(params object[] keyValues)
        {
            var entity = _dbset.Find(keyValues);
            _dbset.Remove(entity);
        }

        public void Remove(TEntity entity)
        {
            _dbset.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbset.RemoveRange(entities);
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbset.Where(predicate);
        }
    }
}
