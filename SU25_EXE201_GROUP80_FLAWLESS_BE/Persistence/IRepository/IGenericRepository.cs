using Domain.Entities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        void AddAsync(T entity);
        void AddRangeAsync(IEnumerable<T> entities);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IQueryable<T>> include);
        IPagedList<T> Find(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllIncludingDeletedAsync();
        Task<T> GetByIdAsync(Guid id);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        IQueryable<Appointment> GetAll();
    }
}
