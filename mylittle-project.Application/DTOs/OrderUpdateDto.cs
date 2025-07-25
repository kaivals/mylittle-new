using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Application.DTOs
{
    public class OrderUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string BuyerName { get; set; } = string.Empty;

        [Required]
        public string Portal { get; set; } = string.Empty;

        [Required]
        public string Dealer { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = string.Empty;

        [Required]
        public string PaymentStatus { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be a positive value.")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [MinLength(1, ErrorMessage = "At least one order item is required.")]
        public List<OrderItemUpdateDto> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }
    }
}
