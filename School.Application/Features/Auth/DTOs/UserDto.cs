namespace School.Application.Features.Auth.DTOs
{
    public class UserDto
    {
        public string UserId { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Token { get; set; } = default!;
        public IList<string> Roles { get; set; } = new List<string>();
        public StudentInfoDto? StudentInfo { get; set; }
    }
}
