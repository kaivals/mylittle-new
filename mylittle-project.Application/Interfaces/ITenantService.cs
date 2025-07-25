using Microsoft.AspNetCore.Mvc;
using mylittle_project.Application.DTOs;
using mylittle_project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mylittle_project.Application.Interfaces
{
    public interface ITenantService
    {
        // ────────────────────────── Core CRUD ──────────────────────────
        Task<IEnumerable<Tenant>> GetAllAsync();
        Task<Tenant> CreateAsync(TenantDto dto);
        Task<Tenant?> GetTenantWithFeaturesAsync(Guid tenantId);

        // ─────────────────────── Feature Management ────────────────────
        /// <summary>Returns the full Module → Feature toggle tree for the UI.</summary>
        Task<List<FeatureModuleDto>> GetFeatureTreeAsync(Guid tenantId);

        /// <summary>Turn an entire module ON/OFF; cascades to all its features.</summary>
        Task<bool> UpdateModuleToggleAsync(Guid tenantId, Guid moduleId, bool isEnabled);

        /// <summary>Turn a single child feature ON/OFF (only allowed if its module is ON).</summary>
        Task<bool> UpdateFeatureToggleAsync(Guid tenantId, Guid featureId, bool isEnabled);

        // ─────────────────────── Store Management ──────────────────────
        Task<bool> UpdateStoreAsync(Guid tenantId, StoreDto dto);

        //licensing and feature management

        Task<List<PortalSummaryDto>> GetPortalSummariesAsync();
        Task<bool> UpdateTenantAsync(Guid tenantId, TenantDto dto);

        Task<PaginatedResult<TenantDto>> GetPaginatedAsync(int page, int pageSize);


    }
}
