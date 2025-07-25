using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Application.DTOs
{
    public class ContentSettingsDto
    {
        [Required(ErrorMessage = "Welcome message is required.")]
        [MaxLength(500, ErrorMessage = "Welcome message cannot exceed 500 characters.")]
        public string WelcomeMessage { get; set; } = string.Empty;

        [Required(ErrorMessage = "Call to action is required.")]
        [MaxLength(300, ErrorMessage = "Call to action cannot exceed 300 characters.")]
        public string CallToAction { get; set; } = string.Empty;

        [Required(ErrorMessage = "Home page content is required.")]
        public string HomePageContent { get; set; } = string.Empty;

        [Required(ErrorMessage = "About us content is required.")]
        public string AboutUs { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact us content is required.")]
        [MaxLength(3000, ErrorMessage = "Contact us content cannot exceed 3000 characters.")]
        public string ContactUs { get; set; } = string.Empty;

        [Required(ErrorMessage = "Terms and privacy policy content is required.")]
        [MaxLength(5000, ErrorMessage = "Terms and privacy policy content cannot exceed 5000 characters.")]
        public string TermsAndPrivacyPolicy { get; set; } = string.Empty;
    }
}
