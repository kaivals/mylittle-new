using mylittle_project.Application.DTOs;
using mylittle_project.Application.Interfaces;
using mylittle_project.Domain.Entities;
using mylittle_project.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace mylittle_project.infrastructure.Services
{
    public class GlobalSubscriptionService : IGlobalSubscriptionService
    {
        private readonly AppDbContext _context;
        public GlobalSubscriptionService(AppDbContext context) => _context = context;

        public async Task<List<GlobalSubscription>> GetAllAsync() =>
            await _context.GlobalSubscriptions.ToListAsync();

        public async Task<GlobalSubscription?> GetByNameAsync(string name) =>
            await _context.GlobalSubscriptions.FirstOrDefaultAsync(p => p.PlanName == name);

        public async Task<GlobalSubscription> CreateAsync(GlobalSubscriptionDto dto)
        {
            var plan = new GlobalSubscription
            {
                Id = Guid.NewGuid(),
                PlanName = dto.PlanName,
                Description = dto.Description,
                PlanCost = dto.PlanCost,
                NumberOfAds = dto.NumberOfAds,
                MaxEssentialMembers = dto.MaxEssentialMembers,
                MaxPremiumMembers = dto.MaxPremiumMembers,
                MaxEliteMembers = dto.MaxEliteMembers,
                IsTrial = dto.IsTrial,
                IsActive = dto.IsActive
            };
            _context.GlobalSubscriptions.Add(plan);
            await _context.SaveChangesAsync();
            return plan;
        }
    }
}
