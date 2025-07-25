using Microsoft.EntityFrameworkCore;
using mylittle_project.Application.DTOs;
using mylittle_project.Application.Interfaces;
using mylittle_project.Domain.Entities;
using mylittle_project.infrastructure.Data;

namespace mylittle_project.infrastructure.Services
{
    public class DealerSubscriptionService : IDealerSubscriptionService
    {
        private readonly AppDbContext _context;

        public DealerSubscriptionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddSubscriptionAsync(DealerSubscriptionDto dto)
        {
            // Get matching tenant plan assignment
            var planAssignment = await _context.TenantPlanAssignments
                .FirstOrDefaultAsync(p =>
                    p.TenantId == dto.TenantId &&
                    p.CategoryId == dto.CategoryId &&
                    p.PlanType == dto.PlanType &&
                    !p.IsDeleted);

            if (planAssignment == null)
                throw new Exception("No tenant plan assignment found for the given category and plan type.");

            var usedSlots = await _context.DealerSubscriptions
                .CountAsync(d =>
                    d.TenantId == dto.TenantId &&
                    d.CategoryId == dto.CategoryId &&
                    d.PlanType == dto.PlanType &&
                    !d.IsQueued &&
                    d.Status != "Expired");

            var isQueued = usedSlots >= planAssignment.MaxSlots;

            var status = isQueued ? "Queued" : "Upcoming";

            var subscription = new DealerSubscription
            {
                Id = Guid.NewGuid(),
                DealerId = dto.DealerId,
                TenantId = dto.TenantId,
                CategoryId = dto.CategoryId,
                PlanType = dto.PlanType,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsQueued = isQueued,
                Status = status
            };

            _context.DealerSubscriptions.Add(subscription);
            await _context.SaveChangesAsync();
        }

        public async Task<List<DealerSubscriptionDto>> GetByTenantAsync(Guid tenantId)
        {
            return await _context.DealerSubscriptions
                .Where(x => x.TenantId == tenantId)
                .Select(x => new DealerSubscriptionDto
                {
                    DealerId = x.DealerId,
                    TenantId = x.TenantId,
                    CategoryId = x.CategoryId,
                    PlanType = x.PlanType,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    IsQueued = x.IsQueued,
                    Status = x.Status
                }).ToListAsync();
        }
    }
}
