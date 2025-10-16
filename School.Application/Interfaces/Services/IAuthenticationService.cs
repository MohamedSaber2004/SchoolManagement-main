using School.Application.Features.Auth.DTOs;

namespace School.Application.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<UserDto> RegisterAsync(RegisterDto registerDto);

        Task<UserDto> LoginAsync(LoginDto loginDto);

        Task<UserDto> GetCurrentUserAsync(string email);

        Task<string> GetUserIdByStudentIdAsync(int studentId);

        Task<StudentAccountDto> CreateStudentAccountAsync(CreateStudentAccountDto createDto, string adminUserId);
    }
}
