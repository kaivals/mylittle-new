using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Application.DTOs
{
    public class OrderItemCreateDto
    {
        [Required(ErrorMessage = "Product name is required.")]
        [MaxLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string ProductName { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }
        public int ProductId { get; set; }
        public decimal? UnitPrice { get; set; }

    }
}
