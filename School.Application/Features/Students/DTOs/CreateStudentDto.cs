namespace School.Application.Features.Students.DTOs
{
    public class CreateStudentDto
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public int? ClassId { get; set; }
    }
}
