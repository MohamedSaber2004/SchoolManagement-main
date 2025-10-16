namespace School.Application.Features.Classes.DTOs
{
    public class UpdateClassDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string TeacherName { get; set; } = default!;
    }
}