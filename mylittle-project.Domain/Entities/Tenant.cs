using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Domain.Entities
{
    public class Tenant : AuditableEntity
    {
        // ────────────────────────── Core fields ─────────────────────────────
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Tenant name is required.")]
        [StringLength(100)]
        public string TenantName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tenant nickname is required.")]
        [StringLength(100)]
        public string TenantNickname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Subdomain is required.")]
        [StringLength(100)]
        public string Subdomain { get; set; } = string.Empty;

        [StringLength(100)]
        public string IndustryType { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTime LastAccessed { get; set; } = DateTime.UtcNow;

        // ─────────────────── Related single-instance entities ───────────────
        public AdminUser? AdminUser { get; set; }
   
        public Store? Store { get; set; }
        public Branding? Branding { get; set; }
        public ContentSettings? ContentSettings { get; set; }
        public DomainSettings? DomainSettings { get; set; }

        // ─────────────────────── NEW dynamic-feature links ───────────────────
        public ICollection<TenantFeatureModule> FeatureModules { get; set; } = new List<TenantFeatureModule>();
        public ICollection<TenantFeature> Features { get; set; } = new List<TenantFeature>();

        // ────────────────────────── Other collections ───────────────────────
        public ICollection<ActivityLogBuyer>? ActivityLogs { get; set; }
        public ICollection<TenantSubscription> TenantSubscriptions { get; set; } = new List<TenantSubscription>();

    }
}
