using mylittle_project.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mylittle_project.Application.Interfaces
{
    public interface IFilterService
    {
        Task<List<FilterDto>> GetAllAsync();
        Task<PaginatedResult<FilterDto>> GetPaginatedAsync(int page, int pageSize);
        Task<FilterDto?> GetByIdAsync(Guid id);
        Task<FilterDto> CreateAsync(CreateFilterDto dto);
        Task<FilterDto?> UpdateAsync(Guid id, CreateFilterDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task SyncFiltersFromProductAsync();

        // ✅ FIXED: Return type updated to List<ProductReadDto>
        Task<List<ProductReadDto>> GetFilteredProductsAsync(Dictionary<string, string> filterValues);
    }
}
