using Microsoft.AspNetCore.Mvc;
using mylittle_project.Application.DTOs;
using mylittle_project.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mylittle_project.API.Controllers
{
    [ApiController]
    [Route("api/dealer")]
    public class DealerController : ControllerBase
    {
        private readonly IDealerService _dealerService;
        private readonly IUserDealerService _userDealerService;
        private readonly IVirtualNumberService _virtualNumberService;
        private readonly IDealerSubscriptionService _dealerSubscriptionService;
        private readonly IKycService _kycService;
        private readonly IProductService _productService; // NEW
        private readonly IFilterService _filterService;   // NEW

        public DealerController(
            IDealerService dealerService,
            IUserDealerService userDealerService,
            IVirtualNumberService virtualNumberService,
            IDealerSubscriptionService dealerSubscriptionService,
            IKycService kycService,
            IProductService productService,
            IFilterService filterService)
        {
            _dealerService = dealerService;
            _userDealerService = userDealerService;
            _virtualNumberService = virtualNumberService;
            _dealerSubscriptionService = dealerSubscriptionService;
            _kycService = kycService;
            _productService = productService;
            _filterService = filterService;
        }

        // ──────────────── BUSINESS INFO ────────────────
        [HttpPost("Dealer")]
        public async Task<IActionResult> CreateBusinessInfo([FromBody] DealerDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _dealerService.CreateBusinessInfoAsync(dto);
            return Ok(new { Message = "Business Info created successfully", DealerId = id });
        }

        // ──────────────── SUB-USERS ────────────────
        [HttpPost("user")]
        public async Task<IActionResult> AddUser([FromBody] UserDealerDto dto)
        {
            var userId = await _userDealerService.AddUserAsync(dto);
            return Ok(new { UserId = userId });
        }

        [HttpPost("user/batch")]
        public async Task<IActionResult> AddMultipleUsers([FromBody] List<UserDealerDto> users)
        {
            if (users == null || users.Count == 0)
                return BadRequest("User list is empty.");

            var createdUserIds = new List<Guid>();

            foreach (var user in users)
            {
                if (user.DealerId == Guid.Empty)
                    continue;

                var id = await _userDealerService.AddUserAsync(user);
                createdUserIds.Add(id);
            }

            return Ok(new
            {
                Message = "Users added successfully.",
                UserIds = createdUserIds
            });
        }

        [HttpGet("user/{DealerId}")]
        public async Task<IActionResult> GetUsers(Guid DealerId)
        {
            var users = await _userDealerService.GetUsersByDealerAsync(DealerId);
            return Ok(users);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userDealerService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("user/paginated")]
        public async Task<IActionResult> GetPaginatedUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var users = await _userDealerService.GetPaginatedUsersAsync(page, pageSize);
            return Ok(users);
        }

        // ──────────────── VIRTUAL NUMBER ────────────────
        [HttpGet("virtual-number/get/{DealerId}")]
        public async Task<IActionResult> GetVirtualNumber(Guid DealerId)
        {
            var number = await _virtualNumberService.GetAssignedNumberAsync(DealerId);
            return Ok(new { virtualNumber = number });
        }

        // ──────────────── SUBSCRIPTION ────────────────
        [HttpPost("subscription/assign")]
        public async Task<IActionResult> AssignDealerSubscription([FromBody] DealerSubscriptionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _dealerSubscriptionService.AddSubscriptionAsync(dto);
                return Ok(new { Message = "Dealer subscription assigned successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        // ──────────────── KYC ────────────────
        [HttpPost("kyc/request")]
        public async Task<IActionResult> AddDocumentRequest([FromBody] KycDocumentRequestDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid request data.");

            await _kycService.AddDocumentRequestAsync(dto);
            return Ok("Document request added successfully.");
        }

        [HttpPost("kyc/upload")]
        public async Task<IActionResult> UploadKycDocument([FromForm] KycDocumentUploadDto dto)
        {
            if (dto.File == null)
                return BadRequest("File is required.");

            var filePath = await _kycService.UploadDocumentAsync(dto);
            return Ok(new { message = "Document uploaded successfully.", filePath });
        }

        [HttpGet("kyc/requested/{DealerId}")]
        public async Task<IActionResult> GetRequestedDocuments(Guid DealerId)
        {
            var docs = await _kycService.GetRequestedDocumentsAsync(DealerId);
            return Ok(docs);
        }

        // ──────────────── 🔥 CREATE PRODUCT FOR DEALER ────────────────
        /// <summary>
        /// Creates a new product for the specified dealer using dynamic product fields.
        /// </summary>
        [HttpPost("{dealerId}/product")]
        public async Task<IActionResult> CreateDealerProduct(Guid dealerId, [FromBody] Dictionary<string, string> fieldValues)
        {
            if (fieldValues == null || fieldValues.Count == 0)
                return BadRequest("No product field values provided.");

            try
            {
                var productId = await _dealerService.CreateProductForDealerAsync(dealerId, fieldValues);

                // 🔄 Auto-sync filters after product is created
                await _filterService.SyncFiltersFromProductAsync();

                return Ok(new { ProductId = productId, Message = "Product created successfully for dealer and filters synced." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
