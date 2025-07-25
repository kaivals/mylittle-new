using Microsoft.EntityFrameworkCore;
using mylittle_project.Application.DTOs;
using mylittle_project.Application.Interfaces;
using mylittle_project.Domain.Entities;
using mylittle_project.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mylittle_project.infrastructure.Services
{
    public class TenantPlanAssignmentService : ITenantPlanAssignmentService
    {
        private readonly AppDbContext _context;

        public TenantPlanAssignmentService(AppDbContext context)
        {
            _context = context;
        }

            public async Task<List<TenantPlanAssignment>> GetByTenantAsync(Guid tenantId)
            {
                return await _context.TenantPlanAssignments
                    .Include(x => x.Category)
                    .Include(x => x.Dealer)
                    .Where(x => x.TenantId == tenantId)
                    .ToListAsync();
            }
        public async Task<List<SchedulerAssignmentDto>> GetSchedulerAssignmentsAsync(Guid tenantId)
        {
            var assignments = await _context.TenantPlanAssignments
                .Include(x => x.Category)
                .Include(x => x.Dealer)
                .Where(x => x.TenantId == tenantId && !x.IsDeleted) // If soft-delete exists
                .ToListAsync();

            var result = assignments.Select(x => new SchedulerAssignmentDto
            {
                Category = x.Category?.Name ?? "N/A",
                Dealer = x.Dealer?.DealerName ?? "N/A",
                PlanType = x.PlanType,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Status = GetSubscriptionStatus(x.StartDate, x.EndDate)
            }).ToList();

            return result;
        }

        private string GetSubscriptionStatus(DateTime startDate, DateTime endDate)
        {
            var today = DateTime.Today;
            if (endDate < today)
                return "Expired";
            if (startDate > today)
                return "Upcoming";
            return "Active";
        }

        public async Task AddAssignmentsAsync(Guid tenantId, List<TenantPlanAssignmentDto> dtos)
        {
            foreach (var dto in dtos)
            {
                _context.TenantPlanAssignments.Add(new TenantPlanAssignment
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    CategoryId = dto.CategoryId,
                    DealerId = dto.DealerId,
                    PlanType = dto.PlanType,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    Status = dto.Status,
                    SlotsUsed = dto.SlotsUsed,
                    MaxSlots = dto.MaxSlots
                });
            }
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAssignmentAsync(Guid id, TenantPlanAssignmentDto dto)
        {
            var existing = await _context.TenantPlanAssignments.FindAsync(id);
            if (existing == null) return false;

            existing.CategoryId = dto.CategoryId;
            existing.DealerId = dto.DealerId;
            existing.PlanType = dto.PlanType;
            existing.StartDate = dto.StartDate;
            existing.EndDate = dto.EndDate;
            existing.Status = dto.Status;
            existing.SlotsUsed = dto.SlotsUsed;
            existing.MaxSlots = dto.MaxSlots;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAssignmentAsync(Guid id)
        {
            var existing = await _context.TenantPlanAssignments.FindAsync(id);
            if (existing == null) return false;

            _context.TenantPlanAssignments.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
