using System;
using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Domain.Entities
{
    public class BrandingMedia : AuditableEntity
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "LogoUrl is required.")]
        [Url(ErrorMessage = "LogoUrl must be a valid URL.")]
        [RegularExpression(@"^https?:\/\/([\w\-]+\.)+[\w\-]+(\/[\w\-._~:/?#[\]@!$&'()*+,;=]*)?$",
            ErrorMessage = "LogoUrl must be a valid HTTP or HTTPS URL.")]
        [MaxLength(300)]
        public string LogoUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "FaviconUrl is required.")]
        [Url(ErrorMessage = "FaviconUrl must be a valid URL.")]
        [RegularExpression(@"^https?:\/\/([\w\-]+\.)+[\w\-]+(\/[\w\-._~:/?#[\]@!$&'()*+,;=]*)?$",
            ErrorMessage = "FaviconUrl must be a valid HTTP or HTTPS URL.")]
        [MaxLength(300)]
        public string FaviconUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "BackgroundImageUrl is required.")]
        [Url(ErrorMessage = "BackgroundImageUrl must be a valid URL.")]
        [RegularExpression(@"^https?:\/\/([\w\-]+\.)+[\w\-]+(\/[\w\-._~:/?#[\]@!$&'()*+,;=]*)?$",
            ErrorMessage = "BackgroundImageUrl must be a valid HTTP or HTTPS URL.")]
        [MaxLength(500)]
        public string BackgroundImageUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "BrandingId is required.")]
        public Guid BrandingId { get; set; }
    }
}
