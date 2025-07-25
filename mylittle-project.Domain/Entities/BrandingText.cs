using System;
using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Domain.Entities
{
    public class BrandingText : AuditableEntity
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Font name is required.")]
        [StringLength(100, ErrorMessage = "Font name can't exceed 100 characters.")]
        public string FontName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Font size is required.")]
        [RegularExpression(@"^\d+(px|em|rem|%)$", ErrorMessage = "FontSize must include a valid unit like px, em, rem, or %.")]
        public string FontSize { get; set; } = string.Empty;

        [Required(ErrorMessage = "Font weight is required.")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "FontWeight must be a numeric value like 400 or 700.")]
        public string FontWeight { get; set; } = string.Empty;
    }
}
