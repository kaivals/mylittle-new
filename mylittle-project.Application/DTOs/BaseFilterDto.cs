using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mylittle_project.Application.DTOs.Common
{

    
    public class BaseFilterDto
    {
        // ─────────────── Basic Filters ───────────────
        public string? Name { get; set; }
        public Guid? ParentId { get; set; }
        public bool? HasProducts { get; set; }
        public bool? HasFilters { get; set; }
        public string? Status { get; set; }

        // ─────────────── 1. Date / Time Filters ───────────────
        public DateTime? DateFilterValue1 { get; set; }
        public DateTime? DateFilterValue2 { get; set; }
        public string? DateFilterOperator { get; set; }
        // Options:
        // - "Is equal to"
        // - "Is not equal to"
        // - "Is after"
        // - "Is after or equal to"
        // - "Is before"
        // - "Is before or equal to"
        // - "Is null"
        // - "Is not null"

        // ─────────────── 2. Vehicle Filters ───────────────
        public string? Vehicle { get; set; }
        public string? VehicleOperator { get; set; }
        // Options:
        // - "Is equal to"
        // - "Is not equal to"
        // - "Is after"
        // - "Is before"

        // ─────────────── 3. Vehicle ID Filters ───────────────
        public string? VehicleId { get; set; }
        public string? VehicleIdOperator { get; set; }
        // Options:
        // - "Contains"
        // - "Is equal to"
        // - "Is not equal to"
        // - "Starts with"
        // - "Ends with"

        // ─────────────── 4. Gallons Per Hour Filters ───────────────
        public double? GallonsPerHour { get; set; }
        public string? GallonsPerHourOperator { get; set; }
        // Options:
        // - "Is equal to"
        // - "Is not equal to"
        // - "Is greater than"
        // - "Is greater than or equal to"
        // - "Is less than"
        // - "Is less than or equal to"

        // ─────────────── Pagination & Sorting ───────────────
        public string? SortBy { get; set; } = "Created";
        public string? SortDirection { get; set; } = "desc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

