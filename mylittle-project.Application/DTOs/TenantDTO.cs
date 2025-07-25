using System;
using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Application.DTOs
{
    public class TenantDto
    {
        public Guid Id { get; set; }

        // ─────────────────────────── Basic info ───────────────────────────
        [Required, MaxLength(100)]
        public string TenantName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tenant nickname is required."), MaxLength(100)]
        public string TenantNickname { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        [RegularExpression(
            @"^[a-z0-9]+(?:-[a-z0-9]+)*$",
            ErrorMessage = "Subdomain must be lowercase, no spaces, and may contain hyphens.")]
        public string Subdomain { get; set; } = string.Empty;

        [MaxLength(50)]
        public string IndustryType { get; set; } = string.Empty;

        [MaxLength(20)]
        [RegularExpression(
            @"^(Active|Inactive|Pending)?$",
            ErrorMessage = "Status must be 'Active', 'Inactive', or 'Pending'.")]
        public string Status { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; }
        public DateTime LastAccessed { get; set; }

        // ─────────────────────────── Relationships ────────────────────────
        public AdminUserDto? AdminUser { get; set; }
      
        public StoreDto? Store { get; set; }
        public BrandingDto? Branding { get; set; }
        public ContentSettingsDto? ContentSettings { get; set; }
        public DomainSettingsDto? DomainSettings { get; set; }
        public List<TenantSubscriptionDto>? TenantSubscriptions { get; set; }
        // Note: FeatureSettingsDto removed — dynamic feature toggles are now handled
        //       via FeatureModuleDto / FeatureToggleDto and not supplied at creation time.
    }
}
