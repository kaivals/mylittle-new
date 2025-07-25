using mylittle_project.Domain.Enums;

namespace mylittle_project.Application.DTOs
{
    public class CreateFilterDto
    {
        public string Name { get; set; } = string.Empty;

        public FilterType Type { get; set; }  // Enum-bound

        public bool IsDefault { get; set; } = false;

        public string Description { get; set; } = string.Empty;

        public List<string>? Values { get; set; }

        public FilterStatus Status { get; set; } = FilterStatus.Active;
    }
}