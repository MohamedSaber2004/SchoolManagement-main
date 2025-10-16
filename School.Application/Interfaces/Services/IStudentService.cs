using School.Application.Common.Models;
using School.Application.Common.Models.QueryParams;
using School.Application.Features.Students.DTOs;

namespace School.Application.Interfaces.Services
{
    public interface IStudentService
    {
        Task<PaginatedResult<StudentDto>> GetAllAsync(StudentQueryParams queryParams);

        Task<StudentDto> GetByIdAsync(int id);

        Task<StudentDto> CreateAsync(CreateStudentDto createStudentDto);

        Task<StudentDto> UpdateAsync(UpdateStudentDto updateStudentDto);

        Task<bool> DeleteAsync(int id);
    }
}
