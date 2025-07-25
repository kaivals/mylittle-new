using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace mylittle_project.Application.DTOs
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    public class DealerDto
    {
        [Required(ErrorMessage = "TenantId is required.")]
        public Guid TenantId { get; set; }

        [Required(ErrorMessage = "DealerName is required.")]
        [StringLength(100, ErrorMessage = "DealerName cannot exceed 100 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-'.]*$", ErrorMessage = "DealerName contains invalid characters.")]
        public string DealerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "BusinessName is required.")]
        [StringLength(150, ErrorMessage = "BusinessName cannot exceed 150 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-&'.]*$", ErrorMessage = "BusinessName contains invalid characters.")]
        public string BusinessName { get; set; } = string.Empty;

        [Required(ErrorMessage = "BusinessNumber is required.")]
        [StringLength(50, ErrorMessage = "BusinessNumber cannot exceed 50 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\-]*$", ErrorMessage = "BusinessNumber must be alphanumeric.")]
        public string BusinessNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "BusinessEmail is required.")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
         ErrorMessage = "Enter a valid email address.")]
        [MaxLength(150, ErrorMessage = "Email cannot exceed 150 characters.")]

        public string BusinessEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "BusinessAddress is required.")]
        [StringLength(200, ErrorMessage = "BusinessAddress cannot exceed 200 characters.")]
        public string BusinessAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact Email is required.")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "Enter a valid email address.")]
        [MaxLength(150, ErrorMessage = "Email cannot exceed 150 characters.")]
        public string ContactEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter a valid 10-digit Indian phone number.")]
        [MaxLength(10, ErrorMessage = "Phone number cannot exceed 10 digits.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Url(ErrorMessage = "Invalid website URL.")]
        [StringLength(200, ErrorMessage = "Website URL is too long.")]
        public string Website { get; set; } = string.Empty;

        [Required(ErrorMessage = "TaxIdOrGstNumber is required.")]
        [StringLength(50, ErrorMessage = "TaxIdOrGstNumber cannot exceed 50 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\-]*$", ErrorMessage = "Invalid Tax ID or GST Number.")]
        public string TaxIdOrGstNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(100, ErrorMessage = "Country cannot exceed 100 characters.")]
        public string Country { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "State cannot exceed 100 characters.")]
        public string State { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required.")]
        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Timezone is required.")]
        [StringLength(100, ErrorMessage = "Timezone cannot exceed 100 characters.")]
        public string Timezone { get; set; } = string.Empty;
    }

}
