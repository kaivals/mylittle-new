using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mylittle_project.Application.DTOs
{
    public class StoreDto
    {
        [Required(ErrorMessage = "Country is required.")]
        [MaxLength(100, ErrorMessage = "Country name must not exceed 100 characters.")]
        public string Country { get; set; } = string.Empty;

        [Required(ErrorMessage = "Currency is required.")]
        [MaxLength(20)]
        [RegularExpression(@"^[A-Z]{3}(?:\s?[-]?\s?[A-Za-z\s]+)?$", ErrorMessage = "Currency must be like 'USD - US Dollar'.")]
        public string Currency { get; set; } = string.Empty;

        [Required(ErrorMessage = "Language is required.")]
        [MaxLength(20, ErrorMessage = "Language must not exceed 20 characters.")]
        public string Language { get; set; } = string.Empty;

        [Required(ErrorMessage = "Timezone is required.")]
        [MaxLength(50, ErrorMessage = "Timezone must not exceed 50 characters.")]
        public string Timezone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Unit system is required.")]
        [MaxLength(50)]
        [RegularExpression(@"^(Metric|Imperial)(\s?\(.*\))?$", ErrorMessage = "Valid values: 'Metric (cm, kg, °C)' or 'Imperial (in, lb, °F)'.")]
        public string UnitSystem { get; set; } = "Metric (cm, kg, °C)";

        [Required(ErrorMessage = "Text direction is required.")]
        [RegularExpression(@"^(LTR|RTL)$", ErrorMessage = "TextDirection must be 'LTR' or 'RTL'.")]
        public string TextDirection { get; set; } = "LTR";

        [Required(ErrorMessage = "Number format with Decimal separator is required.")]
        [MaxLength(20)]
        [RegularExpression(@"^(1,234\.56|1\.234,56|1234\.56|1234,56|1 234,56|1 234\.56)$", ErrorMessage = "Valid formats: '1,234.56', '1.234,56', '1234.56', '1234,56', '1 234,56', '1 234.56'.")]
        public string NumberFormat { get; set; } = "1,234.56";


        [Required(ErrorMessage = "Date format is required.")]
        [MaxLength(20)]
        [RegularExpression(@"^(MM\/DD\/YYYY|DD\/MM\/YYYY|YYYY\-MM\-DD|DD\-MM\-YYYY|MM\-DD\-YYYY|YYYY\/MM\/DD|DD\.MM\.YYYY|YYYY\.MM\.DD)$", ErrorMessage = "Valid formats: MM/DD/YYYY, DD/MM/YYYY, YYYY-MM-DD, DD-MM-YYYY, MM-DD-YYYY, YYYY/MM/DD, DD.MM.YYYY, YYYY.MM.DD")]
        public string DateFormat { get; set; } = "MM/DD/YYYY";


        public bool EnableTaxCalculations { get; set; } = false;
        public bool EnableShipping { get; set; } = false;
        public bool EnableFilters { get; set; } = false;

        
    }
}
