using System;
using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Domain.Entities
{
    public class TenantPlanAssignment : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "TenantId is required.")]
        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }

        [Required(ErrorMessage = "CategoryId is required.")]
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }

        [Required(ErrorMessage = "DealerId is required.")]
        public Guid DealerId { get; set; }
        public Dealer? Dealer { get; set; }

        [Required(ErrorMessage = "PlanType is required.")]
        [RegularExpression("Essential|Premium|Elite", ErrorMessage = "PlanType must be one of: Essential, Premium, or Elite.")]
        public string PlanType { get; set; } = string.Empty;

        [Required(ErrorMessage = "StartDate is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is required.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [RegularExpression("Active|Expired|Upcoming", ErrorMessage = "Status must be Active, Expired, or Upcoming.")]
        public string Status { get; set; } = "Active";

        [Range(0, int.MaxValue, ErrorMessage = "SlotsUsed must be a non-negative number.")]
        public int SlotsUsed { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "MaxSlots must be greater than 0.")]
        public int MaxSlots { get; set; }
    }
}
