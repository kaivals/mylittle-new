using mylittle_project.Application.DTOs;
using mylittle_project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mylittle_project.Application.Interfaces
{
    public interface ITenantPlanAssignmentService
    {
        Task<List<TenantPlanAssignment>> GetByTenantAsync(Guid tenantId);
        Task AddAssignmentsAsync(Guid tenantId, List<TenantPlanAssignmentDto> assignments);
        Task<bool> UpdateAssignmentAsync(Guid assignmentId, TenantPlanAssignmentDto dto);
        Task<bool> DeleteAssignmentAsync(Guid assignmentId);
        Task<List<SchedulerAssignmentDto>> GetSchedulerAssignmentsAsync(Guid tenantId);

    }
}
