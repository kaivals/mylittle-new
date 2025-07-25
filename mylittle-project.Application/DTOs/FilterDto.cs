using mylittle_project.Domain.Enums;

namespace mylittle_project.Application.DTOs
{
    public class FilterDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public FilterType Type { get; set; }

        public bool IsDefault { get; set; }

        public string Description { get; set; } = string.Empty;

        public List<string>? Values { get; set; }

        public FilterStatus Status { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastModified { get; set; }

        public int UsageCount { get; set; }
    }
}