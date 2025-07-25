using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mylittle_project.Application.DTOs
{
    public class ProductTagDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public bool Published { get; set; }
        public int TaggedProducts { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
