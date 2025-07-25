using mylittle_project.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mylittle_project.Application.Interfaces
{
    public interface IProductService
    {
        // Section-related operations
        Task<Guid> CreateSectionAsync(ProductCreateDto dto);
        Task<bool> UpdateSectionAsync(Guid id, ProductCreateDto dto);
        Task<bool> DeleteSectionAsync(Guid id);

        // Field-related operations
        Task<Guid> CreateFieldAsync(ProductFieldDto dto);
        Task<bool> UpdateFieldAsync(Guid id, ProductFieldDto dto);
        Task<bool> DeleteFieldAsync(Guid id);

        // Retrieval
        Task<List<ProductSectionDto>> GetAllSectionsWithFieldsAsync();
        Task<List<ProductSectionDto>> GetDealerVisibleSectionsAsync();

        // Product creation and filtering
        Task<Guid> CreateProductForDealerAsync(Guid dealerId, Dictionary<string, string> fieldValues);
        Task<List<ProductReadDto>> GetFilteredProductsAsync(Dictionary<string, string> filterValues);
    }
}
