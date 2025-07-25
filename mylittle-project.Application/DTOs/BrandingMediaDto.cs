using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Application.DTOs
{
    public class BrandingMediaDto
    {
        [Required(ErrorMessage = "LogoUrl is required.")]
        [Url(ErrorMessage = "LogoUrl must be a valid URL.")]
        [RegularExpression(@"^https?:\/\/([\w\-]+\.)+[\w\-]+(\/[\w\-._~:/?#[\]@!$&'()*+,;=]*)?$",ErrorMessage = "LogoUrl must be a valid HTTP or HTTPS URL.")]
        public string LogoUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "FaviconUrl is required.")]
        [Url(ErrorMessage = "FaviconUrl must be a valid URL.")]
        [RegularExpression(@"^https?:\/\/([\w\-]+\.)+[\w\-]+(\/[\w\-._~:/?#[\]@!$&'()*+,;=]*)?$",ErrorMessage = "FaviconUrl must be a valid HTTP or HTTPS URL.")]
        public string FaviconUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "BackgroundImageUrl is required.")]
        [Url(ErrorMessage = "BackgroundImageUrl must be a valid URL.")]
        [RegularExpression(@"^https?:\/\/([\w\-]+\.)+[\w\-]+(\/[\w\-._~:/?#[\]@!$&'()*+,;=]*)?$",ErrorMessage = "BackgroundImageUrl must be a valid HTTP or HTTPS URL.")]
        public string BackgroundImageUrl { get; set; } = string.Empty;
    }
}
