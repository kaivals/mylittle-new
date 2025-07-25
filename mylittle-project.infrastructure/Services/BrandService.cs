using mylittle_project.Application.DTOs;
using mylittle_project.Application.Interfaces;
using mylittle_project.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace mylittle_project.Infrastructure.Services
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IFeatureAccessService _featureAccess;

        public BrandService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext, IFeatureAccessService featureAccess)
        {
            _unitOfWork = unitOfWork;
            _httpContext = httpContext;
            _featureAccess = featureAccess;
        }

        private Guid GetTenantId()
        {
            var tenantIdHeader = _httpContext.HttpContext?.Request.Headers["Tenant-ID"].FirstOrDefault();
            if (string.IsNullOrEmpty(tenantIdHeader))
                throw new UnauthorizedAccessException("Tenant ID not found in header.");

            return Guid.Parse(tenantIdHeader);
        }

        private async Task EnsureFeatureEnabledAsync()
        {
            var tenantId = GetTenantId();
            var isEnabled = await _featureAccess.IsFeatureEnabledAsync(tenantId, "brand");
            if (!isEnabled)
                throw new UnauthorizedAccessException("Brand feature not enabled for this tenant.");
        }

        public async Task<List<BrandDto>> GetAllAsync()
        {
            await EnsureFeatureEnabledAsync();

            var brands = await _unitOfWork.Brands.GetAllAsync();

            return brands.Select(b => new BrandDto
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                Status = b.Status,
                Order = b.Order,
                Created = b.Created
            }).ToList();
        }

        public async Task<BrandDto?> GetByIdAsync(Guid id)
        {
            await EnsureFeatureEnabledAsync();

            var brand = await _unitOfWork.Brands.GetByIdAsync(id);
            if (brand == null) return null;

            return new BrandDto
            {
                Id = brand.Id,
                Name = brand.Name,
                Description = brand.Description,
                Status = brand.Status,
                Order = brand.Order,
                Created = brand.Created
            };
        }

        public async Task<BrandDto> CreateAsync(CreateBrandDto dto)
        {
            await EnsureFeatureEnabledAsync();

            var brand = new Brand
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                Status = dto.Status,
                Order = dto.Order,
                Created = DateTime.UtcNow
            };

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _unitOfWork.Brands.AddAsync(brand);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            return new BrandDto
            {
                Id = brand.Id,
                Name = brand.Name,
                Description = brand.Description,
                Status = brand.Status,
                Order = brand.Order,
                Created = brand.Created
            };
        }

        public async Task<BrandDto?> UpdateAsync(Guid id, UpdateBrandDto dto)
        {
            await EnsureFeatureEnabledAsync();

            var brand = await _unitOfWork.Brands.GetByIdAsync(id);
            if (brand == null) return null;

            brand.Name = dto.Name;
            brand.Description = dto.Description;
            brand.Status = dto.Status;
            brand.Order = dto.Order;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                _unitOfWork.Brands.Update(brand);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            return new BrandDto
            {
                Id = brand.Id,
                Name = brand.Name,
                Description = brand.Description,
                Status = brand.Status,
                Order = brand.Order,
                Created = brand.Created
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await EnsureFeatureEnabledAsync();

            var brand = await _unitOfWork.Brands.GetByIdAsync(id);
            if (brand == null) return false;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                _unitOfWork.Brands.Remove(brand);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
