using System;
using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Domain.Entities
{
    public class PortalAssignment : AuditableEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Dealer user ID is required.")]
        public Guid DealerUserId { get; set; }

        public UserDealer? DealerUser { get; set; }

        [Required(ErrorMessage = "Assigned portal tenant ID is required.")]
        public Guid AssignedPortalTenantId { get; set; }

        public Tenant? AssignedPortal { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [StringLength(100, ErrorMessage = "Category cannot exceed 100 characters.")]
        public string Category { get; set; } = string.Empty;

        public DateTime AssignedOn { get; set; } = DateTime.UtcNow;
    }
}
