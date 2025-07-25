using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mylittle_project.Application.DTOs
{
    public class CreateProductTagDto
    {
        public string Name { get; set; } = null!;
        public bool Published { get; set; } = true;
    }
}
