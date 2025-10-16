using School.Application.Common.Enums;
using School.Application.Common.Models.QueryParams;
using School.Domain.Entities;

namespace School.Infrastructure.Implementation.Specifications.CourseModuleSpecifications
{
    public class CourseSpecification : BaseSpecification<Course, int>
    {
        public CourseSpecification(CourseQueryParams queryParams)
            : base(c =>
                       (string.IsNullOrWhiteSpace(queryParams.SearchTerm) || c.Name.ToLower().Contains(queryParams.SearchTerm.ToLower())))
        {
            switch (queryParams.SortingOption)
            {
                case SortingOptions.NameAsc:
                    AddOrderBy(c => c.Name);
                    break;
                case SortingOptions.NameDesc:
                    AddOrderByDesc(c => c.Name);
                    break;
                default:
                    break;
            }

            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }

        public CourseSpecification(int id) : base(c => c.Id == id) { }
    }
}
