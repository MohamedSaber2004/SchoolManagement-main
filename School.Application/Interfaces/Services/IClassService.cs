using School.Application.Common.Models;
using School.Application.Common.Models.QueryParams;
using School.Application.Features.Classes.DTOs;

namespace School.Application.Interfaces.Services
{
    public interface IClassService
    {
        Task<PaginatedResult<ClassDto>> GetAllAsync(ClassQueryParams queryParams);

        Task<ClassDto> GetByIdAsync(int id);

        Task<ClassDto> CreateAsync(CreateClassDto createClassDto);

        Task<ClassDto> UpdateAsync(UpdateClassDto updateClassDto);

        Task<bool> DeleteAsync(int id);
    }
}
