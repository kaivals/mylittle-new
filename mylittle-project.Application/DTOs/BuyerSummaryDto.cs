using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//buyersummarydto.cs is a Data Transfer Object (DTO) that represents a summary of buyer information in the application.
//This DTO is used to transfer buyer summary data between different layers of the application efficiently.

namespace mylittle_project.Application.DTOs 
{
    public class BuyerSummaryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        public DateTime RegistrationDate { get; set; }
        public DateTime LastLogin { get; set; }
        public string Status { get; set; } = "Active";
        public bool IsActive { get; set; }

        public Guid DealerId { get; set; }
        public Guid TenantId { get; set; }

        public int TotalOrders { get; set; }
        public int TotalActivities { get; set; }
    }
}
