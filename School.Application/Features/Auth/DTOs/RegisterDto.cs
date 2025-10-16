namespace School.Application.Features.Auth.DTOs
{
    public class RegisterDto
    {
        public string DisplayName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Phone { get; set; } = default!;
    }
}
