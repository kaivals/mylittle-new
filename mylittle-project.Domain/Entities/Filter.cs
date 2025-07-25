using mylittle_project.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Domain.Entities
{
    public class Filter : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid TenantId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public FilterType Type { get; set; }

        public bool IsDefault { get; set; } = false;

        public string Description { get; set; } = string.Empty;

        public List<string> Values { get; set; } = new();  // Stored as JSON via EF Core conversion

        public FilterStatus Status { get; set; } = FilterStatus.Active;

        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        public int UsageCount { get; set; } = 0;

        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}