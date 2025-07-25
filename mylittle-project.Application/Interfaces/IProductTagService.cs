using mylittle_project.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mylittle_project.Application.Interfaces
{
    // Interfaces/IProductTagService.cs
    public interface IProductTagService
    {
        Task<List<ProductTagDto>> GetAllAsync();
        Task<ProductTagDto?> GetByIdAsync(Guid id);
        Task<ProductTagDto> CreateAsync(CreateProductTagDto dto);
        Task<ProductTagDto?> UpdateAsync(Guid id, UpdateProductTagDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> BulkDeleteAsync(List<Guid> ids);
    }

}
