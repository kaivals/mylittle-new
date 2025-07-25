using mylittle_project.Application.DTOs.Common;
using System;

namespace mylittle_project.Application.DTOs
{
    public class OrderFilterDto : BaseFilterDto
    {
        public string? BuyerName { get; set; }
        public string? DealerName { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
    }
}
