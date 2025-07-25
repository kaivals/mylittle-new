using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mylittle_project.Application.DTOs;
using mylittle_project.Application.Interfaces;
using mylittle_project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mylittle_project.Infrastructure.Services
{
    public class ProductAttributeService : IProductAttributeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContext;

        public ProductAttributeService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
        {
            _unitOfWork = unitOfWork;
            _httpContext = httpContext;
        }

        private Guid GetTenantId()
        {
            var tenantIdHeader = _httpContext.HttpContext?.Request.Headers["Tenant-ID"].FirstOrDefault();
            if (tenantIdHeader == null)
                throw new UnauthorizedAccessException("Tenant ID not found in header.");

            return Guid.Parse(tenantIdHeader);
        }

        public async Task<List<ProductAttributeDto>> GetAllAsync()
        {
            var tenantId = GetTenantId();

            var attributes = await _unitOfWork.ProductAttributes
                .GetAll()
                .Where(x => x.TenantId == tenantId)
                .ToListAsync();

            return attributes.Select(x => new ProductAttributeDto
            {
                Id = x.Id,
                Name = x.Name,
                FieldType = x.FieldType,
                IsRequired = x.IsRequired,
                IsVisible = x.IsVisible,
                IsFilterable = x.IsFilterable,
                IsVariantOption = x.IsVariantOption,
                Options = x.Options
            }).ToList();
        }

        public async Task<ProductAttributeDto?> GetByIdAsync(Guid id)
        {
            var tenantId = GetTenantId();

            var attr = await _unitOfWork.ProductAttributes
                .GetAll()
                .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId);

            if (attr == null) return null;

            return new ProductAttributeDto
            {
                Id = attr.Id,
                Name = attr.Name,
                FieldType = attr.FieldType,
                IsRequired = attr.IsRequired,
                IsVisible = attr.IsVisible,
                IsFilterable = attr.IsFilterable,
                IsVariantOption = attr.IsVariantOption,
                Options = attr.Options
            };
        }

        public async Task<Guid> CreateAsync(ProductAttributeDto dto)
        {
            var tenantId = GetTenantId();

            var attribute = new ProductAttribute
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                Name = dto.Name,
                FieldType = dto.FieldType,
                IsRequired = dto.IsRequired,
                IsVisible = dto.IsVisible,
                IsFilterable = dto.IsFilterable,
                IsVariantOption = dto.IsVariantOption,
                Options = dto.Options,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.ProductAttributes.AddAsync(attribute);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
                return attribute.Id;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Guid id, ProductAttributeDto dto)
        {
            var tenantId = GetTenantId();

            var attribute = await _unitOfWork.ProductAttributes
                .GetAll()
                .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId);

            if (attribute == null) return false;

            attribute.Name = dto.Name;
            attribute.FieldType = dto.FieldType;
            attribute.IsRequired = dto.IsRequired;
            attribute.IsVisible = dto.IsVisible;
            attribute.IsFilterable = dto.IsFilterable;
            attribute.IsVariantOption = dto.IsVariantOption;
            attribute.Options = dto.Options;
            attribute.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _unitOfWork.ProductAttributes.Update(attribute);
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

        public async Task<bool> DeleteAsync(Guid id)
        {
            var tenantId = GetTenantId();

            var attribute = await _unitOfWork.ProductAttributes
                .GetAll()
                .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId);

            if (attribute == null) return false;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _unitOfWork.ProductAttributes.Remove(attribute);
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

        public async Task<List<ProductAttributeDto>> GetAutoSyncedAttributesAsync()
        {
            var tenantId = GetTenantId();

            var autoSyncedFields = await _unitOfWork.ProductFields
                .GetAll()
                .Where(f => f.TenantId == tenantId && f.AutoSyncEnabled)
                .ToListAsync();

            var attributes = autoSyncedFields.Select(f => new ProductAttributeDto
            {
                Id = f.Id,
                Name = f.Name,
                FieldType = f.FieldType,
                IsVisible = f.IsVisible,
                IsRequired = f.IsRequired,
                IsFilterable = f.IsFilterable,
                IsVariantOption = f.IsVariantOption,
                IsAutoSynced = true,
                Options = f.Options != null ? string.Join(",", f.Options) : null
            }).ToList();

            return attributes;
        }
    }
}
