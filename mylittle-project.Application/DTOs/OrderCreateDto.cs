using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Application.DTOs
{
    public class OrderCreateDto
    {
        [Required(ErrorMessage = "Order ID is required.")]
        public string OrderId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Buyer name is required.")]
        public string BuyerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Portal name is required.")]
        public string Portal { get; set; } = string.Empty;

        [Required(ErrorMessage = "Dealer name is required.")]
        public string Dealer { get; set; } = string.Empty;

        [Required(ErrorMessage = "Order status is required.")]
        public string Status { get; set; } = string.Empty;

        [Required(ErrorMessage = "Payment status is required.")]
        public string PaymentStatus { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Order date is required.")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Order items are required.")]
        [MinLength(1, ErrorMessage = "At least one item is required.")]
        public List<OrderItemCreateDto> Items { get; set; } = new();
        public Guid BuyerId { get; set; }
        public Guid DealerId { get; set; }
    }
}
