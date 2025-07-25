using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mylittle_project.Domain.Entities
{ 
        public class ActivityLogBuyer : AuditableEntity
    {
            public Guid Id { get; set; } = Guid.NewGuid();

            public Guid TenantId { get; set; }

            public Guid BuyerId { get; set; }

            [Required(ErrorMessage = "Activity is required.")]
            [StringLength(200, ErrorMessage = "Activity description cannot be longer than 200 characters.")]
            public string Activity { get; set; } = string.Empty;

            [Required(ErrorMessage = "Timestamp is required.")]
            public DateTime Timestamp { get; set; } = DateTime.UtcNow;

            [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
            public string Description { get; set; } = string.Empty;

            // Navigation
            public Buyer? Buyer { get; set; }
        }
 }
