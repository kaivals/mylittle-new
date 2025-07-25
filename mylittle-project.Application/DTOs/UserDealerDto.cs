using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Application.DTOs
{
    public class UserDealerDto
    {
        [Required]
        public Guid DealerId { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = "Dealer";

        public bool IsActive { get; set; } = true;

        [Required]
        [MinLength(1, ErrorMessage = "At least one portal assignment is required.")]
        public List<PortalAssignmentDto> PortalAssignments { get; set; } = new();
    }

    public class PortalAssignmentDto
    {
        [Required]
        [StringLength(100)]
        public string PortalName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Category { get; set; } = string.Empty;
    }
}
