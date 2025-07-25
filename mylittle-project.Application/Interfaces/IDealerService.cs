using mylittle_project.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mylittle_project.Application.Interfaces
{
    public interface IDealerService
    {
        /// <summary>
        /// Creates a new dealer business profile.
        /// </summary>
        Task<Guid> CreateBusinessInfoAsync(DealerDto dto);

        Task<Guid> CreateProductForDealerAsync(Guid dealerId, Dictionary<string, string> fieldValues);


    }
}
