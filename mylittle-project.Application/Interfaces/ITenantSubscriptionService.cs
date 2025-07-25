using mylittle_project.Application.DTOs;
using mylittle_project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mylittle_project.Application.Interfaces
{
    public interface ITenantSubscriptionService
    {
        Task<List<TenantSubscription>> GetByTenantAsync(Guid tenantId);
        Task AddCustomPlansToTenantAsync(Guid tenantId, List<TenantSubscriptionDto> plans);
        Task UpdateOrAddPlansAsync(Guid tenantId, List<TenantSubscriptionDto> newPlans);
    }
}
