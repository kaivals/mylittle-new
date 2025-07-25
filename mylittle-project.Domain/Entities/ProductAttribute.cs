using mylittle_project.Domain.Entities;
using System.ComponentModel.DataAnnotations;

public class ProductAttribute : AuditableEntity
{
    public Guid Id { get; set; }

    [Required]
    public Guid TenantId { get; set; }

    [Required(ErrorMessage = "Attribute name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Field type is required.")]
    [StringLength(50)]
    public string FieldType { get; set; } = null!;

    public bool IsRequired { get; set; }
    public bool IsVisible { get; set; }
    public bool IsFilterable { get; set; }
    public bool IsVariantOption { get; set; }

    [StringLength(500)]
    public string? Options { get; set; }

    [StringLength(200)]
    public string? Source { get; set; }

    [StringLength(100)]
    public string? SectionType { get; set; }
}
