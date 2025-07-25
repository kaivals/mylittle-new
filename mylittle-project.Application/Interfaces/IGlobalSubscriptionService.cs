using mylittle_project.Application.DTOs;
using mylittle_project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mylittle_project.Application.Interfaces
{
    public interface IGlobalSubscriptionService
    {
        Task<List<GlobalSubscription>> GetAllAsync();
        Task<GlobalSubscription> CreateAsync(GlobalSubscriptionDto dto);
        Task<GlobalSubscription?> GetByNameAsync(string name);
    }
}
