using Microsoft.EntityFrameworkCore;
using mylittle_project.Application.Interfaces;
using mylittle_project.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mylittle_project.infrastructure.Services
{
    public class FeatureAccessService : IFeatureAccessService
    {
        private readonly AppDbContext _context;

        public FeatureAccessService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsFeatureEnabledAsync(Guid tenantId, string featureKey)
        {
            return await _context.TenantFeatures
                .Include(tf => tf.Feature)
                .Where(tf => tf.TenantId == tenantId && tf.Feature.Key == featureKey && tf.IsEnabled)
                .AnyAsync();
        }
    }

}
