namespace School.Application.Features.Students.DTOs
{
    public class UpdateStudentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public int ClassId { get; set; }
    }
}
