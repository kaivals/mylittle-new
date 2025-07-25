using mylittle_project.Application.DTOs;
using mylittle_project.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mylittle_project.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Guid> CreateOrderAsync(OrderCreateDto dto);
        Task<bool> UpdateOrderAsync(Guid id, OrderUpdateDto dto);
        Task<bool> DeleteOrderAsync(Guid id);

        Task<OrderDto?> GetOrderByIdAsync(Guid id);
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task<PaginatedResult<OrderDto>> GetPaginatedOrdersAsync(OrderFilterDto filter);
    }
}
