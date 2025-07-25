using System;
using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Domain.Entities
{
    public class VirtualNumberAssignment : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Dealer Id is required.")]
        public Guid DealerId { get; set; }

        [Required(ErrorMessage = "VirtualNumber is required.")]
        [StringLength(20, ErrorMessage = "VirtualNumber cannot be longer than 20 characters.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "VirtualNumber must be a 10-digit number.")]
        public string VirtualNumber { get; set; } = string.Empty;

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        public Dealer? Dealer { get; set; }
    }
}
