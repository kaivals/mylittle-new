
using Microsoft.AspNetCore.Mvc;
using mylittle_project.Application.DTOs;
using mylittle_project.Application.Interfaces;
using System.Threading.Tasks;

namespace mylittle_project.API.Controllers
{
   
    [ApiController]
    [Route("api/portal-links")]
    public class TenentPortalLinkController : ControllerBase
    {
        private readonly ITenantPortalLinkService _service;

        public TenentPortalLinkController(ITenantPortalLinkService service)
        {
            _service = service;
        }

        [HttpPost("single")]
        public async Task<IActionResult> AddSingle([FromBody] TenentPortalLinkDto dto)
        {
            await _service.AddLinkAsync(dto);
            return Ok("Single portal link created.");
        }

        [HttpPost("batch")]
        public async Task<IActionResult> AddBatch([FromBody] TenentPortalLinkBatchDto dto)
        {
            await _service.AddLinksBatchAsync(dto);
            return Ok("Multiple portal links created.");
        }

        [HttpGet("linked-portals")]
        public async Task<IActionResult> GetLinkedPortals([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetPaginatedLinkedPortalsAsync(page, pageSize);
            return Ok(result);
        }




    }
}
