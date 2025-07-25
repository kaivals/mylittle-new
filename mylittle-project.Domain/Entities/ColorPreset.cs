using System;
using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Domain.Entities
{
    public class ColorPreset : AuditableEntity
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Preset name is required.")]
        [StringLength(50, ErrorMessage = "Name can't exceed 50 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name must contain only letters and spaces.")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "PrimaryColor must be a valid hex color.")]
        public string PrimaryColor { get; set; } = string.Empty;
        
        [Required]
        [RegularExpression("^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "SecondaryColor must be a valid hex color.")]
        public string SecondaryColor { get; set; } = string.Empty;
        
        [Required]
        [RegularExpression("^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "AccentColor must be a valid hex color.")]
        public string AccentColor { get; set; } = string.Empty;
        public Guid BrandingId { get; set; }
    }
}
