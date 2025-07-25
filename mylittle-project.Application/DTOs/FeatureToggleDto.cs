using System;

namespace mylittle_project.Application.DTOs
{
    public class FeatureToggleDto
    {
        public Guid FeatureId { get; set; }
        public string Name { get; set; } = default!;
        public bool IsEnabled { get; set; }
    }
}
