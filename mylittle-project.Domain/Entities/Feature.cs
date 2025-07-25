using System;
using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Domain.Entities
{
    /// <summary>Concrete feature that can be toggled on/off per tenant.</summary>
    public class Feature : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "ModuleId is required.")]
        public Guid ModuleId { get; set; }

        [Required]
        public FeatureModule Module { get; set; } = default!;

        [Required(ErrorMessage = "Feature key is required.")]
        [StringLength(100, ErrorMessage = "Key cannot exceed 100 characters.")]
        public string Key { get; set; } = default!;

        [Required(ErrorMessage = "Feature name is required.")]
        [StringLength(150, ErrorMessage = "Name cannot exceed 150 characters.")]
        public string Name { get; set; } = default!;

        [StringLength(300, ErrorMessage = "Description cannot exceed 300 characters.")]
        public string? Description { get; set; }

        /// <summary>Marks “premium” items in UI – *not* a toggle itself.</summary>
        public bool IsPremium { get; set; }
    }
}
