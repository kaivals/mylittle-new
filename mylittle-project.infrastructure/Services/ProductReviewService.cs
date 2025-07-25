using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using mylittle_project.Application.DTOs;
using mylittle_project.Application.Interfaces;
using mylittle_project.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace mylittle_project.Infrastructure.Services
{
    public class ProductReviewService : IProductReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IFeatureAccessService _featureAccess;

        public ProductReviewService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext, IFeatureAccessService featureAccess)
        {
            _unitOfWork = unitOfWork;
            _httpContext = httpContext;
            _featureAccess = featureAccess;
        }

        private Guid GetTenantId()
        {
            var tenantId = _httpContext.HttpContext?.Request.Headers["Tenant-ID"].FirstOrDefault();
            if (tenantId == null)
                throw new UnauthorizedAccessException("Tenant ID not found in header.");

            return Guid.Parse(tenantId);
        }

        private async Task EnsureFeatureEnabledAsync()
        {
            var tenantId = GetTenantId();
            if (!await _featureAccess.IsFeatureEnabledAsync(tenantId, "reviews"))
                throw new UnauthorizedAccessException("Review feature not enabled for this tenant.");
        }

        public async Task<List<ProductReviewDto>> GetAllAsync()
        {
            await EnsureFeatureEnabledAsync();

            var reviews = await _unitOfWork.ProductReviews.GetAllAsync();
            var products = await _unitOfWork.Products.GetAllAsync();

            return (from r in reviews
                    join p in products on r.ProductId equals p.Id
                    select new ProductReviewDto
                    {
                        Id = r.Id,
                        ProductName = p.Name,
                        Title = r.Title,
                        ReviewText = r.ReviewText,
                        Rating = r.Rating,
                        IsApproved = r.IsApproved,
                        IsVerified = r.IsVerified,
                        CreatedOn = r.CreatedOn
                    }).ToList();
        }

        public async Task<ProductReviewDto?> GetByIdAsync(Guid id)
        {
            await EnsureFeatureEnabledAsync();

            var review = await _unitOfWork.ProductReviews.GetByIdAsync(id);
            if (review == null) return null;

            var product = await _unitOfWork.Products.GetByIdAsync(review.ProductId);

            return new ProductReviewDto
            {
                Id = review.Id,
                ProductName = product?.Name ?? "Unknown",
                Title = review.Title,
                ReviewText = review.ReviewText,
                Rating = review.Rating,
                IsApproved = review.IsApproved,
                IsVerified = review.IsVerified,
                CreatedOn = review.CreatedOn
            };
        }

        public async Task<ProductReviewDto> CreateAsync(CreateProductReviewDto dto)
        {
            await EnsureFeatureEnabledAsync();

            var entity = new ProductReview
            {
                Id = Guid.NewGuid(),
                ProductId = dto.ProductId,
                Title = dto.Title,
                ReviewText = dto.ReviewText,
                Rating = dto.Rating,
                CreatedOn = DateTime.UtcNow
            };

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _unitOfWork.ProductReviews.AddAsync(entity);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            return await GetByIdAsync(entity.Id) ?? throw new Exception("Error creating review.");
        }

        public async Task<ProductReviewDto?> UpdateAsync(Guid id, UpdateProductReviewDto dto)
        {
            await EnsureFeatureEnabledAsync();

            var entity = await _unitOfWork.ProductReviews.GetByIdAsync(id);
            if (entity == null) return null;

            entity.Title = dto.Title;
            entity.ReviewText = dto.ReviewText;
            entity.Rating = dto.Rating;
            entity.IsApproved = dto.IsApproved;
            entity.IsVerified = dto.IsVerified;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                _unitOfWork.ProductReviews.Update(entity);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await EnsureFeatureEnabledAsync();

            var entity = await _unitOfWork.ProductReviews.GetByIdAsync(id);
            if (entity == null) return false;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                _unitOfWork.ProductReviews.Remove(entity);
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

        public async Task<bool> BulkDeleteAsync(List<Guid> ids)
        {
            await EnsureFeatureEnabledAsync();

            var reviews = _unitOfWork.ProductReviews.GetAll().Where(r => ids.Contains(r.Id)).ToList();

            if (!reviews.Any()) return false;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                _unitOfWork.ProductReviews.RemoveRange(reviews);
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

        public async Task<bool> ApproveAsync(Guid id)
        {
            var review = await _unitOfWork.ProductReviews.GetByIdAsync(id);
            if (review == null) return false;

            review.IsApproved = true;
            _unitOfWork.ProductReviews.Update(review);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DisapproveAsync(Guid id)
        {
            var review = await _unitOfWork.ProductReviews.GetByIdAsync(id);
            if (review == null) return false;

            review.IsApproved = false;
            _unitOfWork.ProductReviews.Update(review);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> VerifyAsync(Guid id)
        {
            var review = await _unitOfWork.ProductReviews.GetByIdAsync(id);
            if (review == null) return false;

            review.IsVerified = true;
            _unitOfWork.ProductReviews.Update(review);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}

