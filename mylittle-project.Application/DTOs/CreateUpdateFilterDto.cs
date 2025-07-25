using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mylittle_project.Application.DTOs
{
    public class CreateUpdateFilterDto
    {
        public string Name { get; set; } = string.Empty;
        public List<string>? Values { get; set; }
    }
}