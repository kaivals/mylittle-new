using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mylittle_project.Application.Interfaces
{
    public interface IFeatureAccessService
    {
        Task<bool> IsFeatureEnabledAsync(Guid tenantId, string featureKey);
    }

}
