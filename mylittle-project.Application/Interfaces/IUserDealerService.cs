using mylittle_project.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mylittle_project.Application.Interfaces
{
    public interface IUserDealerService
    {
        Task<Guid> AddUserAsync(UserDealerDto dto);
        Task<List<UserDealerDto>> GetAllUsersAsync();
        Task<List<UserDealerDto>> GetUsersByDealerAsync(Guid DealerId);

        // ✅ New paginated method
        Task<PaginatedResult<UserDealerDto>> GetPaginatedUsersAsync(int page, int pageSize);
    }
}
