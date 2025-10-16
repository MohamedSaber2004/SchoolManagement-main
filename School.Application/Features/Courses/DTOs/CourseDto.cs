namespace School.Application.Features.Courses.DTOs
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}
