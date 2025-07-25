using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Application.DTOs
{
    public class TenentPortalLinkViewDto
    {
        [Required]
        public Guid SourceTenantId { get; set; }

        [Required]
        public string SourceTenantName { get; set; } = string.Empty;

        [Required]
        public Guid TargetTenantId { get; set; }

        [Required]
        public string TargetTenantName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LinkType { get; set; } = string.Empty;

        public DateTime LinkedSince { get; set; }
    }
}
