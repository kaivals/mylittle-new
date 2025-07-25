using System;
using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Domain.Entities
{
    public class TenentPortalLink : AuditableEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "SourceTenantId is required.")]
        public Guid SourceTenantId { get; set; }

        [Required(ErrorMessage = "TargetTenantId is required.")]
        public Guid TargetTenantId { get; set; }

        [Required(ErrorMessage = "LinkType is required.")]
        [StringLength(100)]
        public string LinkType { get; set; } = string.Empty;

        public DateTime LinkedSince { get; set; } = DateTime.UtcNow;

        public Tenant? SourceTenant { get; set; }
        public Tenant? TargetTenant { get; set; }
    }
}
