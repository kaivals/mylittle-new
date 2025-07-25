using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mylittle_project.Application.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public string BuyerName { get; set; } = string.Empty;
        public string DealerName { get; set; } = string.Empty;

        public int ItemCount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}

