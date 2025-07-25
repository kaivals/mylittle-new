using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mylittle_project.Domain.Entities
{
    public class ProductFieldValue
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }

        public string FieldName { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;

        public Product Product { get; set; } = null!;
    }

}
