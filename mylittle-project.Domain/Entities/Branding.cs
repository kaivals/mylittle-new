using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Domain.Entities
{
    public class Branding :AuditableEntity
    {
        public Guid Id { get; set; }

        // Color settings
        [Required(ErrorMessage = "Primary color is required.")]
        [RegularExpression("^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "PrimaryColor must be a valid hex color.")]
        public string PrimaryColor { get; set; } = string.Empty;

        [Required(ErrorMessage = "Secondary color is required.")]
        [RegularExpression("^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "SecondaryColor must be a valid hex color.")]
        public string SecondaryColor { get; set; } = string.Empty;

        [RegularExpression("^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "AccentColor must be a valid hex color.")]
        public string AccentColor { get; set; } = string.Empty;

        [RegularExpression("^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "BackgroundColor must be a valid hex color.")]
        public string BackgroundColor { get; set; } = string.Empty;

        [RegularExpression("^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "TextColor must be a valid hex color.")]
        public string TextColor { get; set; } = string.Empty;

        public List<ColorPreset>? ColorPresets { get; set; }

        public BrandingText? Text { get; set; }
        public BrandingMedia? Media { get; set; }

        [Required(ErrorMessage = "TenantId is required.")]
        public Guid TenantId { get; set; }
    }
}
