using School.Application.Common.Models.QueryParams;
using School.Domain.Entities;

namespace School.Infrastructure.Implementation.Specifications.CourseModuleSpecifications
{
    public class CoureCountSpecification: BaseSpecification<Course, int>
    {
        public CoureCountSpecification(CourseQueryParams queryParams)
             : base(c =>
                       (string.IsNullOrWhiteSpace(queryParams.SearchTerm) || c.Name.ToLower().Contains(queryParams.SearchTerm.ToLower())))
        {
            
        }
    }
}
