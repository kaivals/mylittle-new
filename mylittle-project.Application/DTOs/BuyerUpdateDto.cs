using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

//buyerupdatedto.cs is a Data Transfer Object (DTO) that represents the data structure for updating buyer information in the application.

namespace mylittle_project.Application.DTOs
{
    public class BuyerUpdateDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name must be 100 characters or less.")]
        public string Name { get; set; } = string.Empty;


        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter a valid 10-digit Indian phone number.")]
        [MaxLength(10, ErrorMessage = "Phone number cannot exceed 10 digits.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(100, ErrorMessage = "Country name must be 100 characters or less.")]
        public string Country { get; set; } = string.Empty;
    }
}
