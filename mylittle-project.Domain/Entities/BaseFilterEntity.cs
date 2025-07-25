using System;

namespace mylittle_project.Domain.Entities
{
    public class BaseFilterEntity
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

        // ─────────────── 2. Vehicle Filters ───────────────
        public string? Vehicle { get; set; }

        // ─────────────── 3. Vehicle ID Filters ───────────────
        public string? VehicleId { get; set; }

        // ─────────────── 4. Gallons Per Hour Filters ───────────────
        public double? GallonsPerHour { get; set; }


    }
}
