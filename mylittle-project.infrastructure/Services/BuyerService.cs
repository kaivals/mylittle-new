using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using mylittle_project.Application.DTOs;
using mylittle_project.Application.DTOs.Common;
using mylittle_project.Application.Interfaces;
using mylittle_project.Domain.Entities;
using System.Linq.Expressions;

namespace mylittle_project.Infrastructure.Services
{
    public class BuyerService : IBuyerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContext;

        public BuyerService(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContext)
        {
            _unitOfWork = unitOfWork;
            _httpContext = httpContext;
        }



        public async Task<Guid> CreateBuyerAsync(BuyerCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Buyer name is required.");

            if (string.IsNullOrWhiteSpace(dto.Email) || !dto.Email.Contains("@"))
                throw new ArgumentException("A valid email is required.");

            var buyer = new Buyer
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Country = dto.Country,
                TenantId = dto.TenantId,
                DealerId = dto.DealerId,
                IsActive = true,
                Status = "Active",
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.Buyers.AddAsync(buyer);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            return buyer.Id;
        }

        public async Task<List<BuyerListDto>> GetAllBuyersAsync()
        {
            return await _unitOfWork.Buyers.Find(b => !b.IsDeleted)
                .Include(b => b.Orders)
                .Select(b => new BuyerListDto
                {
                    Id = b.Id,
                    BuyerName = b.Name,
                    Email = b.Email,
                    PhoneNumber = b.Phone,
                    TotalOrders = b.Orders.Count,
                    TenantId = b.TenantId,
                    DealerId = b.DealerId,
                    IsActive = b.IsActive,
                    Status = b.Status
                })
                .ToListAsync();
        }

        public async Task<PaginatedResult<BuyerListDto>> GetAllBuyersPaginatedAsync(int page, int pageSize)
        {
            Expression<Func<Buyer, bool>> predicate = b => !b.IsDeleted;

            Expression<Func<Buyer, BuyerListDto>> selector = b => new BuyerListDto
            {
                Id = b.Id,
                BuyerName = b.Name,
                Email = b.Email,
                PhoneNumber = b.Phone,
                TotalOrders = b.Orders.Count,
                TenantId = b.TenantId,
                DealerId = b.DealerId,
                IsActive = b.IsActive,
                Status = b.Status
            };

            return await _unitOfWork.Buyers.GetFilteredAsync(predicate, selector, page, pageSize, "CreatedAt", "desc");
        }

        public async Task<List<BuyerListDto>> GetBuyersByBusinessAsync(Guid businessId)
        {
            return await _unitOfWork.Buyers.Find(b => b.DealerId == businessId && !b.IsDeleted)
                .Include(b => b.Orders)
                .Select(b => new BuyerListDto
                {
                    Id = b.Id,
                    BuyerName = b.Name,
                    Email = b.Email,
                    PhoneNumber = b.Phone,
                    TotalOrders = b.Orders.Count,
                    TenantId = b.TenantId,
                    DealerId = b.DealerId,
                    IsActive = b.IsActive,
                    Status = b.Status
                }).ToListAsync();
        }

        public async Task<bool> SoftDeleteBuyerAsync(Guid buyerId)
        {
            var buyer = await _unitOfWork.Buyers.GetByIdAsync(buyerId);
            if (buyer == null || buyer.IsDeleted) return false;

            buyer.IsDeleted = true;
            buyer.DeletedAt = DateTime.UtcNow;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _unitOfWork.Buyers.Update(buyer);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            return true;
        }

        public async Task<BuyerSummaryDto?> GetBuyerProfileAsync(Guid buyerId)
        {
            var buyer = await _unitOfWork.Buyers.Find(b => b.Id == buyerId)
                .Include(b => b.Orders)
                .Include(b => b.ActivityLogs)
                .FirstOrDefaultAsync();

            if (buyer == null || buyer.IsDeleted) return null;

            return new BuyerSummaryDto
            {
                Id = buyer.Id,
                Name = buyer.Name,
                Email = buyer.Email,
                Phone = buyer.Phone,
                Country = buyer.Country,
                RegistrationDate = buyer.RegistrationDate,
                LastLogin = buyer.LastLogin,
                Status = buyer.Status,
                IsActive = buyer.IsActive,
                DealerId = buyer.DealerId,
                TenantId = buyer.TenantId,
                TotalOrders = buyer.Orders?.Count ?? 0,
                TotalActivities = buyer.ActivityLogs?.Count ?? 0
            };
        }

        public async Task<bool> UpdateBuyerAsync(Guid buyerId, BuyerUpdateDto dto)
        {
            var buyer = await _unitOfWork.Buyers.GetByIdAsync(buyerId);
            if (buyer == null || buyer.IsDeleted) return false;

            buyer.Name = dto.Name;
            buyer.Phone = dto.Phone;
            buyer.Country = dto.Country;
            buyer.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _unitOfWork.Buyers.Update(buyer);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            return true;
        }

        public async Task<BuyerListDto?> GetBuyerByIdAsync(Guid id)
        {
            var buyer = await _unitOfWork.Buyers.Find(b => b.Id == id && !b.IsDeleted)
                .Include(b => b.Orders)
                .FirstOrDefaultAsync();

            if (buyer == null)
                return null;

            return new BuyerListDto
            {
                Id = buyer.Id,
                BuyerName = buyer.Name,
                Email = buyer.Email,
                PhoneNumber = buyer.Phone,
                TotalOrders = buyer.Orders.Count,
                TenantId = buyer.TenantId,
                DealerId = buyer.DealerId,
                IsActive = buyer.IsActive,
                Status = buyer.Status
            };
        }
    }
}
