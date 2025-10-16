using School.Domain.Entities;

namespace School.Infrastructure.Implementation.Specifications.StudentModuleSpecifications
{
    public class StudentByEmailSpecification : BaseSpecification<Student,int>
    {
        public StudentByEmailSpecification(string email): base(s => s.Email == email)
        {
            AddInclude(s => s.Class!);
        }
    }
}
