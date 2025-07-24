using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using PagedList;
using Persistence.Data;
using Persistence.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly FlawlessDBContext _context;

        public IQueryable<Appointment> GetAll()
        {
            return _context.Appointments.AsQueryable();
        }

        public GenericRepository(FlawlessDBContext context)
        {
            _context = context;
        }
        public async void AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async void AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
        }

        public async Task<bool> ExistsAsync(Guid Id)
        {
            return await _context.Set<T>().AnyAsync(e => EF.Property<Guid>(e, "Id") == Id);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AnyAsync(predicate);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).ToListAsync();
        }

        public IPagedList<T> Find(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize)
        {
            return _context.Set<T>().Where(predicate).ToPagedList(pageNumber, pageSize);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllIncludingDeletedAsync()
        {
            return await _context.Set<T>().IgnoreQueryFilters().ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid Id)
        {
            return await _context.Set<T>().FindAsync(Id);
        }

        public async void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
        }

        public async Task<IEnumerable<T>> FindAsync(
                        Expression<Func<T, bool>> expression,
                        Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (include != null)
            {
                query = include(query);
            }

            if (expression != null)
            {
                query = query.Where(expression);
            }

            return await query.ToListAsync();
        }
    }
}
