using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Application.DTOs
{
    public class DomainSettingsDto
    {
        [Required(ErrorMessage = "Subdomain is required.")]
        [MaxLength(100, ErrorMessage = "Subdomain must be 100 characters or less.")]
        [RegularExpression(@"^[a-z0-9]+(-[a-z0-9]+)*$", ErrorMessage = "Subdomain must be lowercase and can include numbers and hyphens (not at start or end).")]
        public string Subdomain { get; set; } = string.Empty;

        [MaxLength(200, ErrorMessage = "Custom domain must be 200 characters or less.")]
        [RegularExpression(@"^((https?):\/\/)?([a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,})(\/\S*)?$", ErrorMessage = "Custom domain must be a valid URL.")]
        public string CustomDomain { get; set; } = string.Empty;

        [MaxLength(200, ErrorMessage = "Domain must be 200 characters or less.")]
        [RegularExpression(@"^((https?):\/\/)?([a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,})(\/\S*)?$", ErrorMessage = "Domain must be a valid URL.")]
        public string Domain { get; set; } = string.Empty;

        [MaxLength(200, ErrorMessage = "Main domain must be 200 characters or less.")]
        [RegularExpression(@"^((https?):\/\/)?([a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,})(\/\S*)?$", ErrorMessage = "Main domain must be a valid URL.")]
        public string MainDomain { get; set; } = string.Empty;

        public bool EnableApiAccess { get; set; }
    }
}
