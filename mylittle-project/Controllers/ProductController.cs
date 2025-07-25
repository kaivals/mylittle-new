using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mylittle_project.Application.DTOs;
using mylittle_project.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mylittle_project.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   // [Authorize] // Optional: remove if not using auth
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // 🔹 SECTION APIs -----------------------

        [HttpPost("section")]
        public async Task<IActionResult> CreateSection([FromBody] ProductCreateDto dto)
        {
            var id = await _productService.CreateSectionAsync(dto);
            return Ok(new { SectionId = id });
        }

        [HttpPut("section/{id}")]
        public async Task<IActionResult> UpdateSection(Guid id, [FromBody] ProductCreateDto dto)
        {
            var updated = await _productService.UpdateSectionAsync(id, dto);
            if (!updated) return NotFound("Section not found.");
            return Ok("Section updated successfully.");
        }

        [HttpDelete("section/{id}")]
        public async Task<IActionResult> DeleteSection(Guid id)
        {
            var deleted = await _productService.DeleteSectionAsync(id);
            if (!deleted) return NotFound("Section not found or has existing fields.");
            return Ok("Section deleted successfully.");
        }

        // 🔹 FIELD APIs -------------------------

        [HttpPost("field")]
        public async Task<IActionResult> CreateField([FromBody] ProductFieldDto dto)
        {
            var id = await _productService.CreateFieldAsync(dto);
            return Ok(new { FieldId = id });
        }

        [HttpPut("field/{id}")]
        public async Task<IActionResult> UpdateField(Guid id, [FromBody] ProductFieldDto dto)
        {
            var updated = await _productService.UpdateFieldAsync(id, dto);
            if (!updated) return NotFound("Field not found.");
            return Ok("Field updated successfully.");
        }

        [HttpDelete("field/{id}")]
        public async Task<IActionResult> DeleteField(Guid id)
        {
            var deleted = await _productService.DeleteFieldAsync(id);
            if (!deleted) return NotFound("Field not found.");
            return Ok("Field deleted successfully.");
        }

        // 🔹 GET APIs -------------------------

        [HttpGet("sections")]
        public async Task<IActionResult> GetAllSectionsWithFields()
        {
            var data = await _productService.GetAllSectionsWithFieldsAsync();
            return Ok(data);
        }

        [HttpGet("dealer-visible-sections")]
        public async Task<IActionResult> GetDealerVisibleSections()
        {
            var data = await _productService.GetDealerVisibleSectionsAsync();
            return Ok(data);
        }

        // 🔹 FILTER PRODUCTS -------------------------

        [HttpPost("filter")]
        public async Task<IActionResult> GetFilteredProducts([FromBody] Dictionary<string, string> filterValues)
        {
            var products = await _productService.GetFilteredProductsAsync(filterValues);
            return Ok(products);
        }
    }
}
