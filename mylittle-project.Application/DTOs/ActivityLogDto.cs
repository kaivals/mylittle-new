using System;
using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Application.DTOs
{
    public class ActivityLogDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "TenantId is required.")]
        public int TenantId { get; set; }

        [Required(ErrorMessage = "PerformedBy is required.")]
        [StringLength(100)]
        public string PerformedBy { get; set; } = string.Empty;

        [Required(ErrorMessage = "Action is required.")]
        [StringLength(200)]
        public string Action { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [StringLength(2000)]
        public string Details { get; set; } = string.Empty;
    }
}
