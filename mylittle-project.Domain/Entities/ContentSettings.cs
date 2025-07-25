using System;
using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Domain.Entities
{
    public class ContentSettings : AuditableEntity
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Welcome message is required.")]
        [StringLength(200, ErrorMessage = "Welcome message can't exceed 200 characters.")]
        public string WelcomeMessage { get; set; } = string.Empty;

        [Required(ErrorMessage = "Call to action is required.")]
        [StringLength(150, ErrorMessage = "Call to action can't exceed 150 characters.")]
        public string CallToAction { get; set; } = string.Empty;

        [Required(ErrorMessage = "Home page content is required.")]
        public string HomePageContent { get; set; } = string.Empty;

        [StringLength(5000, ErrorMessage = "About Us content is too long.")]
        public string AboutUs { get; set; } = string.Empty;

        [StringLength(3000, ErrorMessage = "Contact Us content is too long.")]
        public string ContactUs { get; set; } = string.Empty;

        [StringLength(5000, ErrorMessage = "Terms and Privacy Policy content is too long.")]
        public string TermsAndPrivacyPolicy { get; set; } = string.Empty;

        [Required(ErrorMessage = "TenantId is required.")]
        public Guid TenantId { get; set; }
    }
}
