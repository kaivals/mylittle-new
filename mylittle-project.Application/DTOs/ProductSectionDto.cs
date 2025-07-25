using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mylittle_project.Application.DTOs
{
    public class ProductSectionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<ProductFieldDto> Fields { get; set; } = new();
    }

}
