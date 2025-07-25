using mylittle_project.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mylittle_project.Application.Interfaces
{
    public interface IProductAttributeService
    {
        Task<List<ProductAttributeDto>> GetAllAsync();

        Task<ProductAttributeDto?> GetByIdAsync(Guid id);

        Task<Guid> CreateAsync(ProductAttributeDto dto);

        Task<bool> UpdateAsync(Guid id, ProductAttributeDto dto);

        Task<bool> DeleteAsync(Guid id);

        Task<List<ProductAttributeDto>> GetAutoSyncedAttributesAsync();
    }
}
