using mylittle_project.Domain.Entities;
using System.ComponentModel.DataAnnotations;

public class ProductSection : AuditableEntity
{
    public Guid Id { get; set; }

    [Required]
    public Guid TenantId { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
    public string Title { get; set; } = string.Empty;  // e.g., "Product Info", "Shipping"

    public ICollection<ProductField> Fields { get; set; } = new List<ProductField>();

    [Required(ErrorMessage = "Section name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;
}
