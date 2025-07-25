using mylittle_project.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mylittle_project.Application.Interfaces
{
    public interface ITenantPortalLinkService
    {
        Task AddLinkAsync(TenentPortalLinkDto dto); // single
        Task AddLinksBatchAsync(TenentPortalLinkBatchDto dto); // multiple
        Task<IEnumerable<TenentPortalLinkViewDto>> GetAllLinkedPortalsAsync(); // view all links
        Task<IEnumerable<TenantDto>> GetAllTenantsAsync(); // corrected method

        Task<PaginatedResult<TenentPortalLinkDto>> GetPaginatedLinkedPortalsAsync(int page, int pageSize);

    }
}
