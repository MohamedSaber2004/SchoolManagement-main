namespace School.Domain.Entities
{
    public class Enrollment: BaseEntity<int>
    {
        public Student Student { get; set; } = null!;
        public Course Course { get; set; } = null!;

        public int StudentId { get; set; } // FK
        public int CourseId { get; set; } // FK
    }
}
