using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Application.DTOs
{
    public class VirtualNumberAssignmentDto
    {
        [Required]
        public Guid DealerId { get; set; }

        [Required]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Invalid virtual phone number format.")]
        public string VirtualNumber { get; set; } = string.Empty;
    }
}
