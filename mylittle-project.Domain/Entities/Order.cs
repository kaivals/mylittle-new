using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Domain.Entities
{
    public class Order : AuditableEntity
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "BuyerId is required.")]
        public Guid BuyerId { get; set; }

        public Buyer? Buyer { get; set; }

        [Required(ErrorMessage = "DealerId is required.")]
        public Guid DealerId { get; set; }

        public UserDealer? Dealer { get; set; }

        [Required(ErrorMessage = "Portal name is required.")]
        [StringLength(100, ErrorMessage = "Portal name can't exceed 100 characters.")]
        public string Portal { get; set; } = string.Empty;

        [Required(ErrorMessage = "Order status is required.")]
        [StringLength(50, ErrorMessage = "Order status can't exceed 50 characters.")]
        public string OrderStatus { get; set; } = string.Empty;

        [Required(ErrorMessage = "Payment status is required.")]
        [StringLength(50, ErrorMessage = "Payment status can't exceed 50 characters.")]
        public string PaymentStatus { get; set; } = string.Empty;

        [Required(ErrorMessage = "Shipping status is required.")]
        [StringLength(50, ErrorMessage = "Shipping status can't exceed 50 characters.")]
        public string ShippingStatus { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Range(0, double.MaxValue, ErrorMessage = "Total amount must be greater than or equal to 0.")]
        public decimal TotalAmount { get; set; }

        [StringLength(500, ErrorMessage = "Comments can't exceed 500 characters.")]
        public string Comments { get; set; } = string.Empty;

        public ICollection<OrderItem> OrderItems { get; set; }
        public string Status { get; set; }
    }
}
