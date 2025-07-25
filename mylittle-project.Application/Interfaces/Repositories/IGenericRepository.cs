using mylittle_project.Application.DTOs;
using mylittle_project.Application.DTOs.Common;
using mylittle_project.Domain.Entities;
using System.Linq.Expressions;

namespace mylittle_project.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        IQueryable<T> GetAll();
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);

        void Add(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        Task SaveAsync();

        Task<PaginatedResult<TDto>> GetFilteredAsync<TDto>(
            Expression<Func<T, bool>>? filter,
            Expression<Func<T, TDto>> selector,
            int page,
            int pageSize,
            string? sortBy = null,
            string? sortDir = "asc");
       
    }
}
