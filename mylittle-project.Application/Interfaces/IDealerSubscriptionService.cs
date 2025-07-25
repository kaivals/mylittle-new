using mylittle_project.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mylittle_project.Application.Interfaces
{
    public interface IDealerSubscriptionService
    {
        Task AddSubscriptionAsync(DealerSubscriptionDto dto);
        Task<List<DealerSubscriptionDto>> GetByTenantAsync(Guid tenantId);
    }
}
