using System;
using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Domain.Entities
{
    public class Dealer : AuditableEntity
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "TenantId is required.")]
        public Guid TenantId { get; set; }

        public Tenant Tenant { get; set; } = null!;

        [Required(ErrorMessage = "UserDealerId is required.")]
        public Guid UserDealerId { get; set; }

        public UserDealer? UserDealer { get; set; }

        [Required(ErrorMessage = "Dealer name is required.")]
        [StringLength(100, ErrorMessage = "Dealer name can't exceed 100 characters.")]
        public string DealerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Business name is required.")]
        [StringLength(150)]
        public string BusinessName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Business number is required.")]
        [StringLength(20, ErrorMessage = "Business number can't exceed 20 characters.")]
        public string BusinessNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Business email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string BusinessEmail { get; set; } = string.Empty;

        [StringLength(250)]
        public string BusinessAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact email is required.")]
        [EmailAddress(ErrorMessage = "Invalid contact email.")]
        public string ContactEmail { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Url(ErrorMessage = "Website must be a valid URL.")]
        public string Website { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Tax ID can't exceed 50 characters.")]
        public string TaxId { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Country { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string State { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "Timezone can't exceed 100 characters.")]
        public string Timezone { get; set; } = string.Empty;

        public VirtualNumberAssignment? VirtualNumberAssignment { get; set; }
    }
}
