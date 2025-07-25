using Microsoft.EntityFrameworkCore;
using mylittle_project.Application.DTOs;
using mylittle_project.Application.Interfaces.Repositories;
using System.Linq.Expressions;
using LinqKit;
using mylittle_project.infrastructure.Data;

namespace mylittle_project.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.AsExpandable().Where(predicate);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedResult<TDto>> GetFilteredAsync<TDto>(
            Expression<Func<T, bool>>? filter,
            Expression<Func<T, TDto>> selector,
            int page,
            int pageSize,
            string? sortBy = null,
            string? sortDir = "asc")
        {
            IQueryable<T> query = _dbSet.AsExpandable(); // Important

            if (filter != null)
                query = query.Where(filter);

            query = ApplySorting(query, sortBy, sortDir);

            var totalItems = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(selector)
                .ToListAsync();

            return new PaginatedResult<TDto>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };
        }

        private IQueryable<T> ApplySorting(IQueryable<T> query, string? sortBy, string? sortDir)
        {
            if (string.IsNullOrEmpty(sortBy))
                return query;

            var isAsc = sortDir?.ToLower() == "asc";

            try
            {
                var param = Expression.Parameter(typeof(T), "x");
                var property = Expression.PropertyOrField(param, sortBy);
                var lambda = Expression.Lambda(property, param);

                var methodName = isAsc ? "OrderBy" : "OrderByDescending";
                var method = typeof(Queryable).GetMethods()
                    .First(m => m.Name == methodName && m.GetParameters().Length == 2);

                var genericMethod = method.MakeGenericMethod(typeof(T), property.Type);

                return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, lambda })!;
            }
            catch
            {
                return query;
            }
        }
    }
}
