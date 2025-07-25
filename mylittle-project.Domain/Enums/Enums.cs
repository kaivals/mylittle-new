using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mylittle_project.Domain.Enums
{
    // ✅ 1. Core filter types
    public enum FilterType
    {
        Dropdown,       // Single-select dropdown
        MultiSelect,    // Checkbox-style multiselect
        Toggle,         // On/Off toggle
        RangeSlider,    // Slider with min-max range
        Slider,         // Single-value slider
        Text            // Text input filter (optional)
    }

    // ✅ 2. Filter status for controlling visibility/access
    public enum FilterStatus
    {
        Active,
        Inactive,
        Archived
    }

    // ✅ 3. How filter options are managed
    public enum FilterOptionInputType
    {
        Manual,     // Entered manually by admin
        Predefined, // Loaded from predefined list
        External    // Pulled dynamically via API or config
    }
}