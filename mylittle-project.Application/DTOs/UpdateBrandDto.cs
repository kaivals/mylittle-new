using System;

namespace mylittle_project.Application.DTOs
{
    public class UpdateBrandDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int Order { get; set; }
    }
}
