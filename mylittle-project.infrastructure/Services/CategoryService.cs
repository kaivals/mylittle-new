using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using mylittle_project.Application.DTOs;
using mylittle_project.Application.DTOs.Common;
using mylittle_project.Application.Interfaces;
using mylittle_project.Domain.Entities;
using System.Linq.Expressions;

namespace mylittle_project.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFeatureAccessService _featureAccess;
        private readonly IHttpContextAccessor _httpContext;

        public CategoryService(
            IUnitOfWork unitOfWork,
            IFeatureAccessService featureAccess,
            IHttpContextAccessor httpContext)
        {
            _unitOfWork = unitOfWork;
            _featureAccess = featureAccess;
            _httpContext = httpContext;
        }

        private Guid GetTenantId()
        {
            var tenantIdHeader = _httpContext.HttpContext?.Request.Headers["Tenant-ID"].FirstOrDefault();
            if (tenantIdHeader == null)
                throw new UnauthorizedAccessException("Tenant ID not found in header.");

            return Guid.Parse(tenantIdHeader);
        }

        public async Task<PaginatedResult<CategoryDto>> GetFilteredAsync(BaseFilterDto filter)
        {
            var tenantId = GetTenantId();
            var predicate = PredicateBuilder.New<Category>(true);

            predicate = predicate.And(c => c.TenantId == tenantId);

            if (!string.IsNullOrEmpty(filter.Name))
                predicate = predicate.And(c => c.Name.Contains(filter.Name));

            if (!string.IsNullOrEmpty(filter.Status))
                predicate = predicate.And(c => c.Status == filter.Status);

            if (filter.ParentId.HasValue)
                predicate = predicate.And(c => c.ParentId == filter.ParentId);

            if (filter.HasProducts.HasValue)
                predicate = predicate.And(c => c.Products.Any() == filter.HasProducts.Value);

            if (filter.HasFilters.HasValue)
                predicate = predicate.And(c => c.Filters.Any() == filter.HasFilters.Value);

            if (filter.DateFilterOperator == "between" && filter.DateFilterValue1.HasValue && filter.DateFilterValue2.HasValue)
            {
                var start = filter.DateFilterValue1.Value;
                var end = filter.DateFilterValue2.Value;
                predicate = predicate.And(c => c.CreatedAt >= start && c.CreatedAt <= end);
            }
            else if (filter.DateFilterValue1.HasValue)
            {
                var date = filter.DateFilterValue1.Value;
                predicate = filter.DateFilterOperator switch
                {
                    "Is after" => predicate.And(c => c.CreatedAt > date),
                    "Is after or equal to" => predicate.And(c => c.CreatedAt >= date),
                    "Is before" => predicate.And(c => c.CreatedAt < date),
                    "Is before or equal to" => predicate.And(c => c.CreatedAt <= date),
                    "Is equal to" => predicate.And(c => c.CreatedAt.Date == date.Date),
                    "Is not equal to" => predicate.And(c => c.CreatedAt.Date != date.Date),
                    _ => predicate
                };
            }

            Expression<Func<Category, CategoryDto>> selector = c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                Description = c.Description,
                ParentId = c.ParentId,
                ParentName = c.Parent != null ? c.Parent.Name : null,
                ProductCount = c.Products.Count,
                FilterCount = c.Filters.Count,
                Status = c.Status,
                Created = c.CreatedAt,
                Updated = (DateTime)c.UpdatedAt
            };

            return await _unitOfWork.Categories.GetFilteredAsync(
                predicate,
                selector,
                filter.Page,
                filter.PageSize,
                filter.SortBy,
                filter.SortDirection
            );
        }

        public async Task<CategoryDto> GetByIdAsync(Guid id)
        {
            var result = await _unitOfWork.Categories
                .Find(c => c.Id == id)
                .Include(c => c.Parent)
                .Include(c => c.Products)
                .Include(c => c.Filters)
                .AsExpandable()
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Slug = c.Slug,
                    Description = c.Description,
                    ParentId = c.ParentId,
                    ParentName = c.Parent != null ? c.Parent.Name : null,
                    ProductCount = c.Products.Count,
                    FilterCount = c.Filters.Count,
                    Status = c.Status,
                    Created = c.CreatedAt,
                    Updated = (DateTime)c.UpdatedAt
                })
                .FirstOrDefaultAsync();

            return result!;
        }

        public async Task<CategoryDto> CreateAsync(CreateUpdateCategoryDto dto)
        {
            var tenantId = GetTenantId();

            if (!await _featureAccess.IsFeatureEnabledAsync(tenantId, "categories"))
                throw new UnauthorizedAccessException("Category feature not enabled for this tenant.");

            var category = new Category
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                Name = dto.Name,
                Slug = dto.Slug,
                Description = dto.Description,
                ParentId = dto.ParentId,
                Status = dto.Status,
                ProductCount = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.Categories.AddAsync(category);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            return await GetByIdAsync(category.Id);
        }

        public async Task<CategoryDto> UpdateAsync(Guid id, CreateUpdateCategoryDto dto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) return null!;

            category.Name = dto.Name;
            category.Slug = dto.Slug;
            category.Description = dto.Description;
            category.ParentId = dto.ParentId;
            category.Status = dto.Status;
            category.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _unitOfWork.Categories.Update(category);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) return false;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _unitOfWork.Categories.Remove(category);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            return true;
        }

        public Task<PaginatedResult<CategoryDto>> GetAllPaginatedAsync(int page, int pageSize)
        {
            var filter = new BaseFilterDto
            {
                Page = page,
                PageSize = pageSize
            };

            return GetFilteredAsync(filter);
        }
    }
}
