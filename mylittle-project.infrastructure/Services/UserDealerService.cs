using Microsoft.EntityFrameworkCore;
using mylittle_project.Application.DTOs;
using mylittle_project.Application.Interfaces;
using mylittle_project.Domain.Entities;
using mylittle_project.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class UserDealerService : IUserDealerService
{
    private readonly AppDbContext _context;

    public UserDealerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> AddUserAsync(UserDealerDto dto)
    {
        var business = await _context.Dealers
            .Include(b => b.UserDealer)
            .FirstOrDefaultAsync(b => b.Id == dto.DealerId);

        if (business == null)
            throw new Exception("Invalid DealerId provided.");

        var dealerPortalId = business.TenantId;

        var assignments = new List<PortalAssignment>();
        foreach (var pa in dto.PortalAssignments)
        {
            var assignedPortal = await _context.Tenants.FirstOrDefaultAsync(t => t.TenantName == pa.PortalName);
            if (assignedPortal == null) continue;

            var isLinked = await _context.TenentPortalLinks.AnyAsync(link =>
                (link.SourceTenantId == dealerPortalId && link.TargetTenantId == assignedPortal.Id) ||
                (link.SourceTenantId == assignedPortal.Id && link.TargetTenantId == dealerPortalId));

            if (isLinked)
            {
                assignments.Add(new PortalAssignment
                {
                    AssignedPortalTenantId = assignedPortal.Id,
                    Category = pa.Category
                });
            }
        }

        var user = new UserDealer
        {
            Username = dto.Username,
            Role = dto.Role,
            IsActive = dto.IsActive,
            DealerId = business.Id,
            PortalAssignments = assignments
        };

        _context.UserDealers.Add(user);
        await _context.SaveChangesAsync();
        return user.Id;
    }

    public async Task<List<UserDealerDto>> GetAllUsersAsync()
    {
        return await _context.UserDealers
            .Include(u => u.PortalAssignments)
            .ThenInclude(pa => pa.AssignedPortal)
            .Select(u => new UserDealerDto
            {
                DealerId = u.DealerId,
                Username = u.Username,
                Role = u.Role,
                IsActive = u.IsActive,
                PortalAssignments = u.PortalAssignments.Select(pa => new PortalAssignmentDto
                {
                    PortalName = pa.AssignedPortal.TenantName,
                    Category = pa.Category
                }).ToList()
            }).ToListAsync();
    }

    public async Task<List<UserDealerDto>> GetUsersByDealerAsync(Guid DealerId)
    {
        return await _context.UserDealers
            .Include(u => u.PortalAssignments)
            .ThenInclude(pa => pa.AssignedPortal)
            .Where(u => u.DealerId == DealerId)
            .Select(u => new UserDealerDto
            {
                DealerId = u.DealerId,
                Username = u.Username,
                Role = u.Role,
                IsActive = u.IsActive,
                PortalAssignments = u.PortalAssignments.Select(pa => new PortalAssignmentDto
                {
                    PortalName = pa.AssignedPortal.TenantName,
                    Category = pa.Category
                }).ToList()
            }).ToListAsync();
    }

    public async Task<PaginatedResult<UserDealerDto>> GetPaginatedUsersAsync(int page, int pageSize)
    {
        var query = _context.UserDealers
            .Include(u => u.PortalAssignments)
            .ThenInclude(pa => pa.AssignedPortal)
            .Select(u => new UserDealerDto
            {
                DealerId = u.DealerId,
                Username = u.Username,
                Role = u.Role,
                IsActive = u.IsActive,
                PortalAssignments = u.PortalAssignments.Select(pa => new PortalAssignmentDto
                {
                    PortalName = pa.AssignedPortal.TenantName,
                    Category = pa.Category
                }).ToList()
            });

        var totalItems = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResult<UserDealerDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems
        };
    }
}
