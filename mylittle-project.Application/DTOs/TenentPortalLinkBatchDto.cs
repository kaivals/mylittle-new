using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Application.DTOs
{
    public class TenentPortalLinkBatchDto
    {
        [Required]
        public Guid SourceTenantId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one target tenant must be selected.")]
        public List<Guid> TargetTenantIds { get; set; } = new();

        [Required]
        [StringLength(50)]
        public string LinkType { get; set; } = string.Empty;
    }
}
