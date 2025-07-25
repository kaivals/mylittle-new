using System;

namespace mylittle_project.Application.DTOs
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Slug { get; set; }
        public string? Description { get; set; }

        public Guid? ParentId { get; set; }
        public string? ParentName { get; set; }

        public int ProductCount { get; set; }
        public int FilterCount { get; set; }

        public string Status { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
