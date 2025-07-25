using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using mylittle_project.Application.DTOs;

namespace mylittle_project.Application.Interfaces
{
    public interface IProductReviewService
    {
        Task<List<ProductReviewDto>> GetAllAsync();
        Task<ProductReviewDto?> GetByIdAsync(Guid id);
        Task<ProductReviewDto> CreateAsync(CreateProductReviewDto dto);
        Task<ProductReviewDto?> UpdateAsync(Guid id, UpdateProductReviewDto dto);
        Task<bool> DeleteAsync(Guid id);

        Task<bool> ApproveAsync(Guid id);
        Task<bool> DisapproveAsync(Guid id);
        Task<bool> VerifyAsync(Guid id);

        Task<bool> BulkDeleteAsync(List<Guid> ids);
    }
}

