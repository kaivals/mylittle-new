using mylittle_project.Domain.Entities;
using System.ComponentModel.DataAnnotations;

public class Product : AuditableEntity
{
    public Guid Id { get; set; }

    [Required]
    public Guid TenantId { get; set; }

    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(200, ErrorMessage = "Product name cannot exceed 200 characters.")]
    public string Name { get; set; } = string.Empty;

    public ICollection<ProductReview> Reviews { get; set; } = new List<ProductReview>();
    public ICollection<ProductTag> Tags { get; set; } = new List<ProductTag>();
    public Guid DealerId { get; set; }
    public ICollection<ProductFieldValue> FieldValues { get; set; } = new List<ProductFieldValue>();
}
