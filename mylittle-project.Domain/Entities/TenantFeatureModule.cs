using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Domain.Entities
{
    public class TenantFeatureModule : AuditableEntity
    {
        [Required(ErrorMessage = "TenantId is required.")]
        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; } = default!;

        [Required(ErrorMessage = "ModuleId is required.")]
        public Guid ModuleId { get; set; }
        public FeatureModule Module { get; set; } = default!;

        public bool IsEnabled { get; set; }

        public ICollection<TenantFeature> TenantFeatures { get; set; } = new List<TenantFeature>();
    }
}
