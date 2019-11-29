using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Interfaces.Entities;
using Core.Interfaces.Repositories.Sql;
using Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories.Sql
{
    public abstract class Repository<T> : IRepository<T> where T : EntityBase<int>
    {
        private readonly IDbContext _context;
        private readonly DbSet<T> _set;

        protected Repository(IDbContext context)
        {
            _context = context;
            _set = _context.Set<T>();
        }

        /* Utilitários */

        public T GetById(int id)
        {
            return _set.AsNoTracking().FirstOrDefault(e => e.Id == id);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _set.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public IEnumerable<T> All() => _set.AsNoTracking().AsEnumerable();

        public async Task<IEnumerable<T>> AllAsync() => await _set.AsNoTracking().ToListAsync();

        public IEnumerable<T> Find(Expression<Func<T, bool>> where) => _set.AsNoTracking().Where(where);

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> where) => await _set.AsNoTracking().Where(where).ToListAsync();

        public int Count() => _set.AsNoTracking().Count();

        public async Task<int> CountAsync() => await _set.AsNoTracking().CountAsync();

        public IQueryable<T> Query() => _set.AsNoTracking();

        public IQueryable<T> QueryFast() => Query();

        public IQueryable<T> QueryFast(params Expression<Func<T, object>>[] includes)
        {
            var set = QueryFast();
            set = includes.Aggregate(set, (current, include) => current.Include(include));

            return set.AsNoTracking();
        }

        public T Last() => _set.AsNoTracking().LastOrDefault();

        public async Task<T> LastAsync() => await _set.AsNoTracking().LastOrDefaultAsync();

        public IQueryable<T> Query(Expression<Func<T, bool>> predicate, params string[] includes)
        {
            var query = includes.Aggregate(Query(), (current, child) => current.Include(child));
            return query.Where(predicate).AsQueryable();
        }

        public void Dispose()
        {
            _context?.Dispose();
            GC.SuppressFinalize(this);
        }

        /*  */
        public int SaveChanges() => _context.SaveChanges();

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        /* CRUD */

        #region Insert

        public T Insert(T entity, bool autoCommit = true)
        {
            _set.Add(entity);

            if (autoCommit)
            {
                _context.SaveChanges();
            }

            return entity;
        }

        public async Task<T> InsertAsync(T entity, bool autoCommit = true)
        {
            await _set.AddAsync(entity);

            if (autoCommit)
            {
                await _context.SaveChangesAsync();
            }

            return entity;
        }

        public IEnumerable<T> InsertBulk(IEnumerable<T> entity, bool autoCommit = true)
        {
            _set.AddRange(entity);

            if (autoCommit)
            {
                _context.SaveChanges();
            }

            return entity;
        }

        public async Task<IEnumerable<T>> InsertBulkAsync(IEnumerable<T> entity, bool autoCommit = true)
        {
            await _set.AddRangeAsync(entity);

            if (autoCommit)
            {
                await _context.SaveChangesAsync();
            }

            return entity;
        }

        #endregion

        #region Update

        public T Update(T entity, bool autoCommit = true)
        {
            _set.Update(entity);

            if (autoCommit)
            {
                _context.SaveChanges();
            }

            return entity;
        }

        public async Task<T> UpdateAsync(T entity, bool autoCommit = true)
        {
            _set.Update(entity);

            if (autoCommit)
            {
                await _context.SaveChangesAsync();
            }

            return entity;
        }

        public IEnumerable<T> UpdateBulk(IEnumerable<T> entity, bool autoCommit = true)
        {
            _set.UpdateRange(entity);

            if (autoCommit)
            {
                _context.SaveChanges();
            }

            return entity;
        }

        public async Task<IEnumerable<T>> UpdateBulkAsync(IEnumerable<T> entity, bool autoCommit = true)
        {
            _set.UpdateRange(entity);

            if (autoCommit)
            {
                await _context.SaveChangesAsync();
            }

            return entity;
        }

        #endregion

        #region Delete

        public void Delete(T entity, bool autoCommit)
        {
            _set.Remove(entity);

            if (autoCommit)
            {
                _context.SaveChanges();
            }
        }

        public void DeleteBulk(IEnumerable<T> entity, bool autoCommit)
        {
            _set.RemoveRange(entity);

            if (autoCommit)
            {
                _context.SaveChanges();
            }
        }

        public async Task DeleteAsync(T entity, bool autoCommit = true)
        {
            _set.Remove(entity);

            if (autoCommit)
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteBulkAsync(IEnumerable<T> entity, bool autoCommit = true)
        {
            _set.RemoveRange(entity);

            if (autoCommit)
            {
                await _context.SaveChangesAsync();
            }
        }

        #endregion
    }
}