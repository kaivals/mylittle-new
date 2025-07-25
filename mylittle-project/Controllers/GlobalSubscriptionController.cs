using Microsoft.AspNetCore.Mvc;
using mylittle_project.Application.DTOs;
using mylittle_project.Application.Interfaces;

namespace mylittle_project.Controllers
{
    [ApiController]
    [Route("api/global-subscriptions")]
    public class GlobalSubscriptionController : ControllerBase
    {
        private readonly IGlobalSubscriptionService _service;
        public GlobalSubscriptionController(IGlobalSubscriptionService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _service.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GlobalSubscriptionDto dto) =>
            Ok(await _service.CreateAsync(dto));
    }

}
