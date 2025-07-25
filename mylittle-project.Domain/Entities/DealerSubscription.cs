using System;
using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Domain.Entities
{
    public class DealerSubscription : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "DealerId is required.")]
        public Guid DealerId { get; set; }
        public Dealer? Dealer { get; set; }

        [Required(ErrorMessage = "TenantId is required.")]
        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }

        [Required(ErrorMessage = "CategoryId is required.")]
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }

        [Required(ErrorMessage = "Plan type is required.")]
        [RegularExpression("Essential|Premium|Elite", ErrorMessage = "PlanType must be one of: Essential, Premium, Elite.")]
        public string PlanType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        public DateTime EndDate { get; set; }

        public bool IsQueued { get; set; } = false;

        [Required(ErrorMessage = "Status is required.")]
        [RegularExpression("Active|Upcoming|Expired|Queued", ErrorMessage = "Status must be one of: Active, Upcoming, Expired, Queued.")]
        public string Status { get; set; } = "Upcoming";
    }
}
