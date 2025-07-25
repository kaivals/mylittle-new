using System;
using System.Collections.Generic;

namespace mylittle_project.Application.DTOs
{
    public class FeatureModuleDto
    {
        public Guid ModuleId { get; set; }
        public string Name { get; set; } = default!;
        public bool IsEnabled { get; set; }

        public List<FeatureToggleDto> Features { get; set; } = new();
    }
}
