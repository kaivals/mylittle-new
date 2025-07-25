using System;
using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Domain.Entities
{
    public class TenantFeature : AuditableEntity
    {
        [Required(ErrorMessage = "TenantId is required.")]
        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; } = default!;

        [Required(ErrorMessage = "FeatureId is required.")]
        public Guid FeatureId { get; set; }
        public Feature Feature { get; set; } = default!;

        public bool IsEnabled { get; set; }

        [Required(ErrorMessage = "ModuleId is required.")]
        public Guid ModuleId { get; set; }
    }
}
