using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Domain.Entities
{
    /// <summary>High-level module that can group many child features.</summary>
    public class FeatureModule : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Module key is required.")]
        [StringLength(100, ErrorMessage = "Key cannot exceed 100 characters.")]
        public string Key { get; set; } = default!; // Unique code used for lookups/seeds

        [Required(ErrorMessage = "Module name is required.")]
        [StringLength(150, ErrorMessage = "Name cannot exceed 150 characters.")]
        public string Name { get; set; } = default!;

        [StringLength(300, ErrorMessage = "Description cannot exceed 300 characters.")]
        public string? Description { get; set; }

        // Navigation – one module => many features
        public ICollection<Feature> Features { get; set; } = new List<Feature>();
    }
}
