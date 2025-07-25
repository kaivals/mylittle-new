using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mylittle_project.Application.DTOs
{
    public class ProductReadDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public Dictionary<string, object> Attributes { get; set; } = new();
        public Guid CategoryId { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
        public string Data { get; set; } = string.Empty;
        public Guid DealerId { get; set; }
        public Dictionary<string, string>? Fields { get; set; }
    }

}
