using mylittle_project.Domain.Entities;
using System.ComponentModel.DataAnnotations;

public class ProductTag : AuditableEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "Tag name is required.")]
    [MaxLength(100, ErrorMessage = "Tag name cannot exceed 100 characters.")]
    public string Name { get; set; } = null!;

    public bool Published { get; set; } = true;

    [Range(0, int.MaxValue, ErrorMessage = "Tagged product count cannot be negative.")]
    public int TaggedProducts { get; set; } = 0;

    public DateTime Created { get; set; } = DateTime.UtcNow;

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public Product Product { get; set; } = null!;
}
