using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using mylittle_project.Application.DTOs;

namespace mylittle_project.Application.Interfaces
{
    public interface IBrandService
    {
        Task<List<BrandDto>> GetAllAsync();
        Task<BrandDto?> GetByIdAsync(Guid id);
        Task<BrandDto> CreateAsync(CreateBrandDto dto);
        Task<BrandDto?> UpdateAsync(Guid id, UpdateBrandDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
