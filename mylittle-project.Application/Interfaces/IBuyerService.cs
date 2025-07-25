using mylittle_project.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mylittle_project.Application.Interfaces
{
    public interface IBuyerService
    {
        Task<Guid> CreateBuyerAsync(BuyerCreateDto dto);
        Task<List<BuyerListDto>> GetAllBuyersAsync();
        Task<List<BuyerListDto>> GetBuyersByBusinessAsync(Guid DealerId);
        Task<BuyerListDto?> GetBuyerByIdAsync(Guid id);
        Task<bool> SoftDeleteBuyerAsync(Guid buyerId);
        Task<bool> UpdateBuyerAsync(Guid buyerId, BuyerUpdateDto dto);
        Task<BuyerSummaryDto?> GetBuyerProfileAsync(Guid buyerId);
        Task<PaginatedResult<BuyerListDto>> GetAllBuyersPaginatedAsync(int page, int pageSize);
    }
}