using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Application.DTOs
{
    public class BrandingTextDto
    {
        [Required(ErrorMessage = "Font name is required.")]
        [StringLength(100, ErrorMessage = "Font name must not exceed 100 characters.")]
        public string FontName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Font size is required.")]
        [RegularExpression(@"^\d+(px|em|rem|%)$", ErrorMessage = "FontSize must include units (e.g. 16px, 1em, 90%).")]
        public string FontSize { get; set; } = string.Empty;

        [Required(ErrorMessage = "Font weight is required.")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "FontWeight must be a numeric string like 400 or 700.")]
        public string FontWeight { get; set; } = string.Empty;
    }
}
