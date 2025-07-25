using System;
using System.Collections.Generic;

namespace mylittle_project.Application.DTOs
{
    public class ProductFieldDto
    {
        public Guid Id { get; set; }                      // For Update
        public Guid SectionId { get; set; }               // Foreign key to ProductSection

        public string Name { get; set; } = string.Empty;  // Field name
        public string FieldType { get; set; } = string.Empty; // text, dropdown, etc.

        public bool IsRequired { get; set; } = false;           // Toggle: Required
        public bool AutoSyncEnabled { get; set; } = false;      // Toggle: Auto
        public bool IsVisibleToDealer { get; set; } = true;     // For dealer filtering
        public bool VisibleToDealer { get; set; } = true;       // (you had both — kept for compatibility)

        // ✅ NEW FIELDS: For filtering logic
        public bool IsFilterable { get; set; } = false;         // Toggle: Filtering
        public bool IsVariantOption { get; set; } = false;      // Toggle: Variants
        public bool IsVisible { get; set; } = true;             // Toggle: Visible in UI

        public List<string>? Options { get; set; }              // Used when FieldType == "dropdown"
    }
}
