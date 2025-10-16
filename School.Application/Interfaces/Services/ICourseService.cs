using School.Application.Common.Models;
using School.Application.Common.Models.QueryParams;
using School.Application.Features.Courses.DTOs;

namespace School.Application.Interfaces.Services
{
    public interface ICourseService
    {
        Task<PaginatedResult<CourseDto>> GetAllAsync(CourseQueryParams queryParams);

        Task<CourseDto> GetByIdAsync(int id);

        Task<CourseDto> CreateAsync(CreateCourseDto courseDto);

        Task<CourseDto> UpdateAsync(UpdateCourseDto courseDto);

        Task<bool> DeleteAsync(int id);
    }
}
