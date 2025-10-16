using School.Application.Common.Models.QueryParams;
using School.Domain.Entities;

namespace School.Infrastructure.Implementation.Specifications.StudentModuleSpecifications
{
    public class StudentsCountSpecification: BaseSpecification<Student,int>
    {
        public StudentsCountSpecification(StudentQueryParams queryParams)
            : base(s =>
                       (!queryParams.ClassId.HasValue || s.ClassId == queryParams.ClassId)
                    && (string.IsNullOrWhiteSpace(queryParams.SearchTerm) || s.Name.ToLower().Contains(queryParams.SearchTerm.ToLower())))
        {
            
        }
    }
}
