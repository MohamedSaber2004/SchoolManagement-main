namespace School.Domain.Entities
{
    public class Class:BaseEntity<int>
    {
        public string Name { get; set; } = null!;
        public string TeacherName { get; set; } = null!;
    }
}
