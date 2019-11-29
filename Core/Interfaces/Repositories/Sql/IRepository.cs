using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Interfaces.Entities;

namespace Core.Interfaces.Repositories.Sql
{
    public interface IRepository<T> 
    {
        /* CRUD */

        T Insert(T entity, bool autoCommit = true);
        Task<T> InsertAsync(T entity, bool autoCommit = true);

        IEnumerable<T> InsertBulk(IEnumerable<T> entity, bool autoCommit = true);
        Task<IEnumerable<T>> InsertBulkAsync(IEnumerable<T> entity, bool autoCommit = true);

        T Update(T entity, bool autoCommit = true);
        Task<T> UpdateAsync(T entity, bool autoCommit = true);

        IEnumerable<T> UpdateBulk(IEnumerable<T> entity, bool autoCommit = true);
        Task<IEnumerable<T>> UpdateBulkAsync(IEnumerable<T> entity, bool autoCommit = true);

        void Delete(T entity, bool autoCommit = true);
        Task DeleteAsync(T entity, bool autoCommit = true);

        void DeleteBulk(IEnumerable<T> entity, bool autoCommit = true);
        Task DeleteBulkAsync(IEnumerable<T> entity, bool autoCommit = true);

        /* Utilitários */

        T GetById(int id);
        Task<T> GetByIdAsync(int id);

        IEnumerable<T> All();
        Task<IEnumerable<T>> AllAsync();

        IEnumerable<T> Find(Expression<Func<T, bool>> where);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> where);

        int Count();
        Task<int> CountAsync();

        T Last();
        Task<T> LastAsync();

        IQueryable<T> Query();
        IQueryable<T> Query(Expression<Func<T, bool>> predicate, params string[] includes);
        IQueryable<T> QueryFast();
        IQueryable<T> QueryFast(params Expression<Func<T, object>>[] includes);
    }
}