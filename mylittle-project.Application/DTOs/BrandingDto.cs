using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Application.DTOs
{
    public class BrandingDto
    {
        [Required(ErrorMessage = "Primary color is required.")]
        [RegularExpression("^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "PrimaryColor must be a valid hex code.")]
        public string PrimaryColor { get; set; } = string.Empty;

        [Required(ErrorMessage = "Secondary color is required.")]
        [RegularExpression("^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "SecondaryColor must be a valid hex code.")]
        public string SecondaryColor { get; set; } = string.Empty;

        [Required(ErrorMessage = "Accent color is required.")]
        [RegularExpression("^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "AccentColor must be a valid hex code.")]
        public string AccentColor { get; set; } = string.Empty;

        [Required(ErrorMessage = "Background color is required.")]
        [RegularExpression("^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "BackgroundColor must be a valid hex code.")]
        public string BackgroundColor { get; set; } = string.Empty;

        [Required(ErrorMessage = "Text color is required.")]
        [RegularExpression("^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "TextColor must be a valid hex code.")]
        public string TextColor { get; set; } = string.Empty;

        public List<ColorPresetDto> ColorPresets { get; set; } = new();

        public BrandingTextDto? TextSettings { get; set; }

        public BrandingMediaDto? MediaSettings { get; set; }
    }
}
