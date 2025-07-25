using mylittle_project.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ProductReview : AuditableEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "Product ID is required.")]
    public Guid ProductId { get; set; }

    [Required(ErrorMessage = "Review title is required.")]
    [MaxLength(150, ErrorMessage = "Title cannot exceed 150 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Review text is required.")]
    [StringLength(1000, ErrorMessage = "Review text can't exceed 1000 characters.")]
    public string ReviewText { get; set; } = string.Empty;

    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
    public int Rating { get; set; }

    public bool IsApproved { get; set; }
    public bool IsVerified { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    [ForeignKey("ProductId")]
    public Product? Product { get; set; }
}
