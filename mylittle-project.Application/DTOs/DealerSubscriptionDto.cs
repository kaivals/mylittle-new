using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mylittle_project.Application.DTOs
{
    public class DealerSubscriptionDto
    {
        public Guid DealerId { get; set; }
        public Guid TenantId { get; set; }
        public Guid CategoryId { get; set; }

        public string PlanType { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsQueued { get; set; }
        public string Status { get; set; } = "Upcoming";
    }
}
