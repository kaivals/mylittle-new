using mylittle_project.Domain.Entities;
using System.ComponentModel.DataAnnotations;

public class ProductField : AuditableEntity
{
    public Guid Id { get; set; }

    [Required]
    public Guid TenantId { get; set; }

    [Required(ErrorMessage = "Field name is required.")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Field type is required.")]
    [StringLength(50)]
    public string FieldType { get; set; } = string.Empty;

    [Required]
    public Guid SectionId { get; set; }

    public ProductSection Section { get; set; } = null!;

    public bool IsRequired { get; set; }
    public bool IsFilterable { get; set; }
    public bool IsVariantOption { get; set; }
    public bool IsVisible { get; set; }
    public bool AutoSyncEnabled { get; set; }

    public List<string>? Options { get; set; }

    public bool IsVisibleToDealer { get; set; }


    
}
