using Microsoft.AspNetCore.Mvc;
using mylittle_project.Application.DTOs;
using mylittle_project.Application.Interfaces;
using mylittle_project.infrastructure.Services;

[ApiController]
[Route("api/[controller]")]


public class TenantPlanAssignmentController : ControllerBase
{
    private readonly ITenantPlanAssignmentService _service;

    public TenantPlanAssignmentController(ITenantPlanAssignmentService service)
    {
        _service = service;
    }

    [HttpGet("{tenantId}")]
    public async Task<IActionResult> Get(Guid tenantId)
    {
        var plans = await _service.GetByTenantAsync(tenantId);
        return Ok(plans);
    }
    [HttpGet("scheduler/{tenantId}")]
    public async Task<IActionResult> GetSchedulerAssignments(Guid tenantId)
    {
        var result = await _service.GetSchedulerAssignmentsAsync(tenantId);
        return Ok(result);
    }

    [HttpPost("{tenantId}")]
    public async Task<IActionResult> Add(Guid tenantId, [FromBody] List<TenantPlanAssignmentDto> assignments)
    {
        await _service.AddAssignmentsAsync(tenantId, assignments);
        return Ok("Assignments added.");
    }

    [HttpPut("{assignmentId}")]
    public async Task<IActionResult> Update(Guid assignmentId, [FromBody] TenantPlanAssignmentDto dto)
    {
        var result = await _service.UpdateAssignmentAsync(assignmentId, dto);
        if (!result) return NotFound("Assignment not found.");
        return Ok("Updated successfully.");
    }

    [HttpDelete("{assignmentId}")]
    public async Task<IActionResult> Delete(Guid assignmentId)
    {
        var result = await _service.DeleteAssignmentAsync(assignmentId);
        if (!result) return NotFound("Assignment not found.");
        return Ok("Deleted.");
    }
}
