namespace School.Domain.Entities
{
    public class Student:BaseEntity<int>
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;

        public Class? Class { get; set; } = null!;
        public ICollection<Enrollment> Enrollments   { get; set; } = [];

        public int? ClassId { get; set; } // FK
    }
}
