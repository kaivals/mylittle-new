using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Application.DTOs
{
    public class ColorPresetDto
    {
        [Required(ErrorMessage = "Preset name is required.")]
        [MaxLength(50, ErrorMessage = "Name must be 50 characters or less.")]
        [RegularExpression(@"^[a-zA-Z0-9_\-\s]{2,50}$", ErrorMessage = "Preset name must be alphanumeric and 2–50 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Primary color is required.")]
        [RegularExpression("^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "Primary color must be a valid hex code.")]
        public string PrimaryColor { get; set; } = string.Empty;

        [Required(ErrorMessage = "Secondary color is required.")]
        [RegularExpression("^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "Secondary color must be a valid hex code.")]
        public string SecondaryColor { get; set; } = string.Empty;

        [Required(ErrorMessage = "Accent color is required.")]
        [RegularExpression("^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "Accent color must be a valid hex code.")]
        public string AccentColor { get; set; } = string.Empty;
    }
}
