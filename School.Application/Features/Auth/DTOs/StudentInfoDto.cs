namespace School.Application.Features.Auth.DTOs
{
    public class StudentInfoDto
    {
        public int StudentId { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public int? ClassId { get; set; }
    }
}