using Microsoft.AspNetCore.Mvc;
using mylittle_project.Application.DTOs;
using mylittle_project.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mylittle_project.Controllers
{
    /// <summary>
    /// Endpoints used by the “Portal Licensing & Feature Access” screen.
    /// </summary>
    [ApiController]
    [Route("api/tenant-feature-settings")]
    public class LicensingFeatureController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public LicensingFeatureController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        // ───────────────────────────────────────────────────────────────
        // 1) LEFT COLUMN: List all portals with enabled-module counts
        //    GET  /api/tenant-feature-settings/portals?page=1&pageSize=10
        //    Returns: [{ tenantId, tenantName, modulesOn, modulesAll, ... }]
        // ───────────────────────────────────────────────────────────────
        [HttpGet("portals")]
        public async Task<ActionResult<PaginatedResult<PortalSummaryDto>>> GetPortals(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var allPortals = await _tenantService.GetPortalSummariesAsync(); // full objects: [{ tenantId, tenantName, modulesOn, modulesAll, ... }]

            var totalItems = allPortals.Count;
            var items = allPortals
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new PaginatedResult<PortalSummaryDto>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return Ok(result);
        }


        // ───────────────────────────────────────────────────────────────
        // 2) RIGHT PANEL: Module → feature tree for a selected portal
        //    GET  /api/tenant-feature-settings/{tenantId}/features
        // ───────────────────────────────────────────────────────────────
        [HttpGet("{tenantId:guid}/features")]
        public async Task<ActionResult<List<FeatureModuleDto>>> GetFeatureTree(Guid tenantId)
        {
            var tree = await _tenantService.GetFeatureTreeAsync(tenantId);
            return tree.Count == 0 ? NotFound() : Ok(tree);
        }

        // ───────────────────────────────────────────────────────────────
        // 3) Master toggle (entire module)
        //    PUT  /api/tenant-feature-settings/{tenantId}/modules/{moduleId}
        //    Body: { "isEnabled": true/false }
        // ───────────────────────────────────────────────────────────────
        [HttpPut("{tenantId:guid}/modules/{moduleId:guid}")]
        public async Task<IActionResult> ToggleModule(
            Guid tenantId,
            Guid moduleId,
            [FromBody] ToggleDto body)
        {
            var ok = await _tenantService.UpdateModuleToggleAsync(
                         tenantId, moduleId, body.IsEnabled);
            return ok ? NoContent() : BadRequest("Module not found or rule violated.");
        }

        // ───────────────────────────────────────────────────────────────
        // 4) Child toggle (single feature)
        //    PUT  /api/tenant-feature-settings/{tenantId}/features/{featureId}
        //    Body: { "isEnabled": true/false }
        // ───────────────────────────────────────────────────────────────
        [HttpPut("{tenantId:guid}/features/{featureId:guid}")]
        public async Task<IActionResult> ToggleFeature(
            Guid tenantId,
            Guid featureId,
            [FromBody] ToggleDto body)
        {
            var ok = await _tenantService.UpdateFeatureToggleAsync(
                         tenantId, featureId, body.IsEnabled);
            return ok ? NoContent() : BadRequest("Feature not found or parent OFF.");
        }
    }

    /// <summary>Simple body for the toggle endpoints.</summary>
    public record ToggleDto(bool IsEnabled);
}
