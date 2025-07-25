using Microsoft.AspNetCore.Mvc;
using mylittle_project.Application.DTOs;
using mylittle_project.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class TenantSubscriptionController : ControllerBase
{
    private readonly ITenantSubscriptionService _service;

    public TenantSubscriptionController(ITenantSubscriptionService service)
    {
        _service = service;
    }

    // GET: /api/TenantSubscription/{tenantId}
    [HttpGet("{tenantId}")]
    public async Task<IActionResult> Get(Guid tenantId)
    {
        var result = await _service.GetByTenantAsync(tenantId);
        return Ok(result);
    }

    // POST: /api/TenantSubscription/{tenantId}
    [HttpPost("{tenantId}")]
    public async Task<IActionResult> UpdatePlans(Guid tenantId, [FromBody] List<TenantSubscriptionDto> plans)
    {
        await _service.UpdateOrAddPlansAsync(tenantId, plans);
        return Ok(new { message = "Plans added or updated successfully." });
    }

}
