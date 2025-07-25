using System;

namespace mylittle_project.Application.DTOs
{
    public class ProductAttributeDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string FieldType { get; set; } = string.Empty;

        public bool IsRequired { get; set; }

        public bool IsVisible { get; set; }

        public bool IsFilterable { get; set; }

        public bool IsVariantOption { get; set; }

        public bool IsAutoSynced { get; set; } // for distinguishing auto synced attributes

        public string? Options { get; set; }
    }
}
