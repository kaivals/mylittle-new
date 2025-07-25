using Microsoft.AspNetCore.Http;
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
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFeatureAccessService _featureAccessService;
        private readonly IHttpContextAccessor _httpContext;

        public ProductService(
            IUnitOfWork unitOfWork,
            IFeatureAccessService featureAccessService,
            IHttpContextAccessor httpContext)
        {
            _unitOfWork = unitOfWork;
            _featureAccessService = featureAccessService;
            _httpContext = httpContext;
        }

        private Guid GetTenantId()
        {
            var tenantId = _httpContext.HttpContext?.Request.Headers["Tenant-ID"].FirstOrDefault();
            if (tenantId == null)
                throw new UnauthorizedAccessException("Tenant ID not found in header.");

            return Guid.Parse(tenantId);
        }

        public async Task<Guid> CreateProductForDealerAsync(Guid dealerId, Dictionary<string, string> fieldValues)
        {
            var tenantId = GetTenantId();

            if (!await _featureAccessService.IsFeatureEnabledAsync(tenantId, "products"))
                throw new UnauthorizedAccessException("Product feature not enabled for this tenant.");

            var visibleFields = await _unitOfWork.ProductFields.GetAll()
                .Where(f => f.TenantId == tenantId && f.IsVisibleToDealer)
                .ToListAsync();

            var allowedFieldNames = visibleFields.Select(f => f.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);

            var validFieldValues = fieldValues
                .Where(kv => allowedFieldNames.Contains(kv.Key))
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            if (!validFieldValues.Any())
                throw new InvalidOperationException("No valid visible product fields were provided by the dealer.");

            var product = new Product
            {
                Id = Guid.NewGuid(),
                DealerId = dealerId,
                TenantId = tenantId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                FieldValues = validFieldValues.Select(kv => new ProductFieldValue
                {
                    Id = Guid.NewGuid(),
                    FieldName = kv.Key,
                    Value = kv.Value
                }).ToList()
            };

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.Products.AddAsync(product);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
                return product.Id;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<Guid> CreateSectionAsync(ProductCreateDto dto)
        {
            var tenantId = GetTenantId();

            if (!await _featureAccessService.IsFeatureEnabledAsync(tenantId, "products"))
                throw new UnauthorizedAccessException("Product feature not enabled for this tenant.");

            var section = new ProductSection
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                Name = dto.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.ProductSections.AddAsync(section);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
                return section.Id;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> UpdateSectionAsync(Guid id, ProductCreateDto dto)
        {
            var tenantId = GetTenantId();

            var section = await _unitOfWork.ProductSections.GetAll()
                .FirstOrDefaultAsync(s => s.Id == id && s.TenantId == tenantId);

            if (section == null) return false;

            section.Name = dto.Name;
            section.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _unitOfWork.ProductSections.Update(section);
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

        public async Task<bool> DeleteSectionAsync(Guid id)
        {
            var tenantId = GetTenantId();

            var section = await _unitOfWork.ProductSections.GetAll()
                .Include(s => s.Fields)
                .FirstOrDefaultAsync(s => s.Id == id && s.TenantId == tenantId);

            if (section == null || section.Fields.Any())
                return false;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _unitOfWork.ProductSections.Remove(section);
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

        public async Task<Guid> CreateFieldAsync(ProductFieldDto dto)
        {
            var tenantId = GetTenantId();

            if (!await _featureAccessService.IsFeatureEnabledAsync(tenantId, "products"))
                throw new UnauthorizedAccessException("Product feature not enabled for this tenant.");

            var fieldId = Guid.NewGuid();

            var field = new ProductField
            {
                Id = fieldId,
                TenantId = tenantId,
                SectionId = dto.SectionId,
                Name = dto.Name,
                FieldType = dto.FieldType,
                IsVisibleToDealer = dto.IsVisibleToDealer,
                IsRequired = dto.IsRequired,
                AutoSyncEnabled = dto.AutoSyncEnabled,
                IsFilterable = dto.IsFilterable,
                IsVariantOption = dto.IsVariantOption,
                IsVisible = dto.IsVisible,
                Options = dto.Options,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.ProductFields.AddAsync(field);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
                return fieldId;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> UpdateFieldAsync(Guid id, ProductFieldDto dto)
        {
            var tenantId = GetTenantId();

            var field = await _unitOfWork.ProductFields.GetAll()
                .FirstOrDefaultAsync(f => f.Id == id && f.TenantId == tenantId);

            if (field == null) return false;

            field.Name = dto.Name;
            field.SectionId = dto.SectionId;
            field.FieldType = dto.FieldType;
            field.IsVisibleToDealer = dto.IsVisibleToDealer;
            field.IsRequired = dto.IsRequired;
            field.AutoSyncEnabled = dto.AutoSyncEnabled;
            field.IsFilterable = dto.IsFilterable;
            field.IsVariantOption = dto.IsVariantOption;
            field.IsVisible = dto.IsVisible;
            field.Options = dto.Options;
            field.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _unitOfWork.ProductFields.Update(field);
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

        public async Task<bool> DeleteFieldAsync(Guid id)
        {
            var tenantId = GetTenantId();

            var field = await _unitOfWork.ProductFields.GetAll()
                .FirstOrDefaultAsync(f => f.Id == id && f.TenantId == tenantId);

            if (field == null) return false;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _unitOfWork.ProductFields.Remove(field);
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

        public async Task<List<ProductSectionDto>> GetAllSectionsWithFieldsAsync()
        {
            var tenantId = GetTenantId();

            var sections = await _unitOfWork.ProductSections.GetAll()
                .Include(s => s.Fields)
                .Where(s => s.TenantId == tenantId)
                .ToListAsync();

            return sections.Select(s => new ProductSectionDto
            {
                Id = s.Id,
                Name = s.Name,
                Fields = s.Fields.Select(f => new ProductFieldDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    FieldType = f.FieldType,
                    SectionId = f.SectionId,
                    IsVisibleToDealer = f.IsVisibleToDealer,
                    IsRequired = f.IsRequired,
                    AutoSyncEnabled = f.AutoSyncEnabled,
                    IsFilterable = f.IsFilterable,
                    IsVariantOption = f.IsVariantOption,
                    IsVisible = f.IsVisible,
                    Options = f.Options
                }).ToList()
            }).ToList();
        }

        public async Task<List<ProductSectionDto>> GetDealerVisibleSectionsAsync()
        {
            var tenantId = GetTenantId();

            var sections = await _unitOfWork.ProductSections.GetAll()
                .Include(s => s.Fields)
                .Where(s => s.TenantId == tenantId)
                .ToListAsync();

            return sections.Select(s => new ProductSectionDto
            {
                Id = s.Id,
                Name = s.Name,
                Fields = s.Fields
                    .Where(f => f.IsVisibleToDealer)
                    .Select(f => new ProductFieldDto
                    {
                        Id = f.Id,
                        Name = f.Name,
                        FieldType = f.FieldType,
                        SectionId = f.SectionId,
                        IsVisibleToDealer = f.IsVisibleToDealer,
                        IsRequired = f.IsRequired,
                        AutoSyncEnabled = f.AutoSyncEnabled,
                        IsFilterable = f.IsFilterable,
                        IsVariantOption = f.IsVariantOption,
                        IsVisible = f.IsVisible,
                        Options = f.Options
                    }).ToList()
            }).ToList();
        }

        public async Task<List<ProductReadDto>> GetFilteredProductsAsync(Dictionary<string, string> filterValues)
        {
            var tenantId = GetTenantId();

            var products = await _unitOfWork.Products.GetAll()
                .Include(p => p.FieldValues)
                .Where(p => p.TenantId == tenantId)
                .ToListAsync();

            var result = new List<ProductReadDto>();

            foreach (var product in products)
            {
                var values = product.FieldValues.ToDictionary(fv => fv.FieldName, fv => fv.Value, StringComparer.OrdinalIgnoreCase);
                bool matchesAll = filterValues.All(f => values.TryGetValue(f.Key, out var val) && val.Equals(f.Value, StringComparison.OrdinalIgnoreCase));

                if (matchesAll)
                {
                    result.Add(new ProductReadDto
                    {
                        Id = product.Id,
                        DealerId = product.DealerId,
                        Fields = values
                    });
                }
            }

            return result;
        }
    }
}
