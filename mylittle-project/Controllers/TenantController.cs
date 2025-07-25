using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mylittle_project.Application.DTOs;
using mylittle_project.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mylittle_project.Controllers
{
    
    [ApiController]
    [Route("api/v1/tenants")]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public TenantsController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        // ───────────────────────────────────────────────────────────────
        // 1)  GET  /api/v1/tenants
        // ───────────────────────────────────────────────────────────────
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<TenantDto>>> GetAllAsync(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _tenantService.GetPaginatedAsync(page, pageSize);
            return Ok(result);
        }

        [HttpGet("{tenantId:guid}", Name = "GetTenantById")]
        public async Task<ActionResult<TenantDto>> GetTenantById(Guid tenantId)
        {
            var tenant = await _tenantService.GetTenantWithFeaturesAsync(tenantId);
            return tenant == null ? NotFound() : Ok(tenant);
        }



        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody] TenantDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var tenant = await _tenantService.CreateAsync(dto);
            return CreatedAtRoute("GetTenantById", new { tenantId = tenant.Id }, tenant);
        }
        // ───────────────────────────────────────────────────────────────
        //  Put  /api/v1/tenants/{tenantId}/update
        // ───────────────────────────────────────────────────────────────
        [HttpPut("{tenantId}")]
        public async Task<IActionResult> UpdateTenant(Guid tenantId, [FromBody] TenantDto dto)
        {
            var success = await _tenantService.UpdateTenantAsync(tenantId, dto);
            if (!success)
                return NotFound("Tenant not found or update failed.");

            return Ok("Tenant updated successfully.");
        }


        [HttpGet("{tenantId:guid}/features")]
        public async Task<ActionResult<List<FeatureModuleDto>>> GetFeatureTreeAsync(Guid tenantId)
        {
            var tree = await _tenantService.GetFeatureTreeAsync(tenantId);
            return tree.Count > 0 ? Ok(tree) : NotFound();
        }

        [HttpPut("{tenantId:guid}/modules/{moduleId:guid}")]
        public async Task<ActionResult> ToggleModuleAsync(Guid tenantId, Guid moduleId, [FromBody] ToggleDto dto)
        {
            var ok = await _tenantService.UpdateModuleToggleAsync(tenantId, moduleId, dto.IsEnabled);
            return ok ? NoContent() : BadRequest("Toggle failed (module not found or parent rule).");
        }

        [HttpPut("{tenantId:guid}/features/{featureId:guid}")]
        public async Task<ActionResult> ToggleFeatureAsync(Guid tenantId, Guid featureId, [FromBody] ToggleDto dto)
        {
            var ok = await _tenantService.UpdateFeatureToggleAsync(tenantId, featureId, dto.IsEnabled);
            return ok ? NoContent() : BadRequest("Toggle failed (feature not found or parent OFF).");
        }

        [HttpPut("{tenantId:guid}/store")]
        public async Task<ActionResult> UpdateStoreAsync(Guid tenantId, [FromBody] StoreDto dto)
        {
            var ok = await _tenantService.UpdateStoreAsync(tenantId, dto);
            return ok ? NoContent() : NotFound();
        }

        public record ToggleDto
        {
            public bool IsEnabled { get; init; }
        }
    }
}
