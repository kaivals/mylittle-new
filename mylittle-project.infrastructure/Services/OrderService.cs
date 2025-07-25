using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using mylittle_project.Application.DTOs;
using mylittle_project.Application.DTOs.Common;
using mylittle_project.Application.Interfaces;
using mylittle_project.Domain.Entities;
using System.Linq.Expressions;

namespace mylittle_project.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContext;

        public OrderService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
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

        public async Task<Guid> CreateOrderAsync(OrderCreateDto dto)
        {
            var tenantId = GetTenantId();

            var order = new Order
            {
                Id = Guid.NewGuid(),
                BuyerId = dto.BuyerId,
                DealerId = dto.DealerId,
                Portal = dto.Portal,
                OrderStatus = dto.Status,
                PaymentStatus = dto.PaymentStatus,
                ShippingStatus = "Pending",
                TotalAmount = dto.Amount,
                Comments = "",
                OrderDate = dto.OrderDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false,
                Status = "Active",
                OrderItems = dto.Items.Select(i => new OrderItem
                {
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.Orders.AddAsync(order);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
                return order.Id;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> UpdateOrderAsync(Guid id, OrderUpdateDto dto)
        {
            var order = await _unitOfWork.Orders
                .GetAll()
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return false;

            order.OrderStatus = dto.Status;
            order.PaymentStatus = dto.PaymentStatus;
            order.TotalAmount = dto.Amount;
            order.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _unitOfWork.Orders.Update(order);
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

        public async Task<OrderDto?> GetOrderByIdAsync(Guid id)
        {
            var order = await _unitOfWork.Orders
                .GetAll()
                .Include(o => o.Buyer)
                .Include(o => o.Dealer)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return null;

            return new OrderDto
            {
                Id = order.Id,
                CreatedAt = order.CreatedAt,
                BuyerName = order.Buyer?.Name ?? "",
                DealerName = order.Dealer?.Name ?? "",
                ItemCount = order.OrderItems.Count,
                TotalAmount = order.TotalAmount
            };
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Orders
                .GetAll()
                .Include(o => o.Buyer)
                .Include(o => o.Dealer)
                .Include(o => o.OrderItems)
                .ToListAsync();

            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                CreatedAt = o.CreatedAt,
                BuyerName = o.Buyer?.Name ?? "",
                DealerName = o.Dealer?.Name ?? "",
                ItemCount = o.OrderItems.Count,
                TotalAmount = o.TotalAmount
            }).ToList();
        }

        public async Task<PaginatedResult<OrderDto>> GetPaginatedOrdersAsync(OrderFilterDto filter)
        {
            Expression<Func<Order, bool>> predicate = PredicateBuilder.New<Order>(true);

            if (!string.IsNullOrWhiteSpace(filter.BuyerName))
                predicate = predicate.And(o => o.Buyer.Name.Contains(filter.BuyerName));
            if (!string.IsNullOrWhiteSpace(filter.DealerName))
                predicate = predicate.And(o => o.Dealer.Name.Contains(filter.DealerName));
            if (filter.CreatedFrom.HasValue)
                predicate = predicate.And(o => o.CreatedAt >= filter.CreatedFrom.Value);
            if (filter.CreatedTo.HasValue)
                predicate = predicate.And(o => o.CreatedAt <= filter.CreatedTo.Value);

            return await _unitOfWork.Orders.GetFilteredAsync(
                predicate,
                selector: o => new OrderDto
                {
                    Id = o.Id,
                    CreatedAt = o.CreatedAt,
                    BuyerName = o.Buyer.Name,
                    DealerName = o.Dealer.Name,
                    ItemCount = o.OrderItems.Count,
                    TotalAmount = o.TotalAmount
                },
                page: filter.Page,
                pageSize: filter.PageSize,
                sortBy: filter.SortBy,
                sortDir: filter.SortDirection
            );
        }

        public async Task<bool> DeleteOrderAsync(Guid id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null) return false;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _unitOfWork.Orders.Remove(order);
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
    }
}
