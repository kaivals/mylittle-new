using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Domain.Entities
{
    public class Store : AuditableEntity
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(100, ErrorMessage = "Country name can't exceed 100 characters.")]
        public string Country { get; set; } = string.Empty;

        [Required(ErrorMessage = "Currency is required.")]
        [StringLength(20, ErrorMessage = "Currency format is too long.")]
        [RegularExpression(@"^[A-Z]{3}(?:\s?[-]?\s?[A-Za-z\s]+)?$", ErrorMessage = "Currency must be in format like 'USD - US Dollar'.")]
        public string Currency { get; set; } = string.Empty;

        [Required(ErrorMessage = "Language is required.")]
        [StringLength(50, ErrorMessage = "Language must not exceed 50 characters.")]
        public string Language { get; set; } = string.Empty;

        [Required(ErrorMessage = "Timezone is required.")]
        [StringLength(100, ErrorMessage = "Timezone must not exceed 100 characters.")]
        public string Timezone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Unit system is required.")]
        [StringLength(50, ErrorMessage = "Unit system must not exceed 50 characters.")]
        [RegularExpression(@"^(Metric|Imperial)(\s?\(.*\))?$", ErrorMessage = "Valid values: 'Metric (cm, kg, °C)' or 'Imperial (in, lb, °F)'.")]
        public string UnitSystem { get; set; } = "Metric (cm, kg, °C)";

        [Required(ErrorMessage = "Text direction is required.")]
        [RegularExpression(@"^(LTR|RTL)$", ErrorMessage = "TextDirection must be 'LTR' or 'RTL'.")]
        public string TextDirection { get; set; } = "LTR";

        [Required(ErrorMessage = "Number format is required.")]
        [StringLength(20)]
        [RegularExpression(@"^(\d{1,3}(,\d{3})*|\d+)(\.\d+)?$", ErrorMessage = "Use number format like '1,234.56'.")]
        public string NumberFormat { get; set; } = "1,234.56";

        [Required(ErrorMessage = "Date format is required.")]
        [StringLength(20)]
        [RegularExpression(@"^(MM\/DD\/YYYY|DD\/MM\/YYYY)$", ErrorMessage = "Valid formats: 'MM/DD/YYYY' or 'DD/MM/YYYY'.")]
        public string DateFormat { get; set; } = "MM/DD/YYYY";

        public bool EnableTaxCalculations { get; set; } = false;

        public bool EnableShipping { get; set; } = false;

        public bool EnableFilters { get; set; } = false;

        [Required(ErrorMessage = "TenantId is required.")]
        public Guid TenantId { get; set; }

       
    }
}
