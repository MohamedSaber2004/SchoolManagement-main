namespace School.Domain.Entities
{
    public class Course: BaseEntity<int>
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public ICollection<Enrollment> Enrollments { get; set; } = [];
    }
}
