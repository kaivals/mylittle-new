using System;
using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Domain.Entities
{
    public class DomainSettings : AuditableEntity
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Subdomain is required.")]
        [RegularExpression(@"^[a-z0-9-]+$", ErrorMessage = "Subdomain must be lowercase letters, numbers, or hyphens only.")]
        [StringLength(100, ErrorMessage = "Subdomain can't exceed 100 characters.")]
        public string Subdomain { get; set; } = string.Empty;

        [Url(ErrorMessage = "CustomDomain must be a valid URL.")]
        public string CustomDomain { get; set; } = string.Empty;

        [Url(ErrorMessage = "Domain must be a valid URL.")]
        public string Domain { get; set; } = string.Empty;

        [Url(ErrorMessage = "MainDomain must be a valid URL.")]
        public string MainDomain { get; set; } = string.Empty;

        public bool EnableApiAccess { get; set; }
        public Guid TenantId { get; set; }
    }
}
