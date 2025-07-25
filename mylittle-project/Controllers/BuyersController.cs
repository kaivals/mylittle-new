using Microsoft.AspNetCore.Mvc;
using mylittle_project.Application.DTOs;
using mylittle_project.Application.Interfaces;

namespace mylittle_project.Controllers
{
    [ApiController]
    [Route("api/dealer/buyers")]
    public class BuyersController : ControllerBase
    {
        private readonly IBuyerService _buyerService;

        public BuyersController(IBuyerService buyerService)
        {
            _buyerService = buyerService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BuyerCreateDto dto)
        {
            var id = await _buyerService.CreateBuyerAsync(dto);
            return Ok(new { BuyerId = id });
        }

        [HttpPut("{buyerId}")]
        public async Task<IActionResult> Update(Guid buyerId, [FromBody] BuyerUpdateDto dto)
        {
            await _buyerService.UpdateBuyerAsync(buyerId, dto);
            return Ok("Buyer updated successfully.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var buyers = await _buyerService.GetAllBuyersPaginatedAsync(page, pageSize);
            return Ok(buyers);
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUnfiltered()
        {
            var buyers = await _buyerService.GetAllBuyersAsync();
            return Ok(buyers);
        }


        [HttpGet("{buyerId}")]
        public async Task<IActionResult> GetProfile(Guid buyerId)
        {
            var profile = await _buyerService.GetBuyerProfileAsync(buyerId);
            if (profile == null) return NotFound("Buyer not found.");

            return Ok(profile);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            var result = await _buyerService.SoftDeleteBuyerAsync(id);
            if (!result)
                return NotFound("Buyer not found or already deleted.");

            return Ok("Buyer soft-deleted successfully.");
        }

    }
}
