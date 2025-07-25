using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mylittle_project.Application.DTOs
{
    public class TenantPlanAssignmentDto
    {
        public Guid CategoryId { get; set; }
        public Guid DealerId { get; set; }
        public string PlanType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "Active";
        public int SlotsUsed { get; set; }
        public int MaxSlots { get; set; }
    }
    public class SchedulerAssignmentDto
    {
        public string Category { get; set; } = string.Empty;
        public string Dealer { get; set; } = string.Empty;
        public string PlanType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }

}
