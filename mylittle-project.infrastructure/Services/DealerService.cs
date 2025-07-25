using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using mylittle_project.Application.DTOs;
using mylittle_project.Application.Interfaces;
using mylittle_project.Domain.Entities;
using mylittle_project.infrastructure.Data;

namespace mylittle_project.infrastructure.Services
{
    public class DealerService : IDealerService
    {
        private readonly AppDbContext _context;

        public DealerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateBusinessInfoAsync(DealerDto dto)
        {
            var dealerUser = new UserDealer
            {
                Username = dto.BusinessEmail,
                Role = "Dealer",
                IsActive = true
            };
            _context.UserDealers.Add(dealerUser);

            var dealer = new Dealer
            {
                TenantId = dto.TenantId,
                DealerName = dto.DealerName,
                BusinessName = dto.BusinessName,
                BusinessNumber = dto.BusinessNumber,
                BusinessEmail = dto.BusinessEmail,
                BusinessAddress = dto.BusinessAddress,
                ContactEmail = dto.ContactEmail,
                PhoneNumber = dto.PhoneNumber,
                Website = dto.Website,
                TaxId = dto.TaxIdOrGstNumber,
                Country = dto.Country,
                State = dto.State,
                City = dto.City,
                Timezone = dto.Timezone,
                UserDealer = dealerUser
            };

            _context.Dealers.Add(dealer);
            await _context.SaveChangesAsync();

            var virtualNumber = "VN" + DateTime.UtcNow.Ticks.ToString().Substring(5, 10);

            var virtualAssignment = new VirtualNumberAssignment
            {
                DealerId = dealer.Id,
                VirtualNumber = virtualNumber
            };

            _context.VirtualNumberAssignments.Add(virtualAssignment);
            await _context.SaveChangesAsync();

            return dealer.Id;
        }

        public async Task<Guid> CreateProductForDealerAsync(Guid dealerId, Dictionary<string, string> fieldValues)
        {
            var dealer = await _context.Dealers
                .Include(d => d.UserDealer)
                .FirstOrDefaultAsync(d => d.Id == dealerId);

            if (dealer == null)
                throw new ArgumentException("Dealer not found.");

            var tenantId = dealer.TenantId;

            var visibleFields = await _context.ProductFields
                .Where(f => f.TenantId == tenantId && f.IsVisibleToDealer)
                .ToListAsync();

            var allowedFieldNames = visibleFields
                .Select(f => f.Name)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var validFieldValues = fieldValues
                .Where(kv => allowedFieldNames.Contains(kv.Key))
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            if (!validFieldValues.Any())
                throw new InvalidOperationException("No valid visible product fields were provided by the dealer.");

            // ✅ First create product
            var product = new Product
            {
                Id = Guid.NewGuid(),
                DealerId = dealerId,
                TenantId = tenantId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // ✅ Then assign values to product
            product.FieldValues = validFieldValues.Select(kv => new ProductFieldValue
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                FieldName = kv.Key,
                Value = kv.Value
            }).ToList();

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product.Id;
        }


    }
}
