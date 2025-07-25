using mylittle_project.Application.DTOs;
using mylittle_project.Application.DTOs.Common;
using System.Threading.Tasks;

namespace mylittle_project.Application.Interfaces
{
    public interface IBaseFilterService<TDto, in TFilterDto>
        where TFilterDto : BaseFilterDto
    {
        Task<PaginatedResult<TDto>> GetFilteredAsync(TFilterDto filter);
    }
}
