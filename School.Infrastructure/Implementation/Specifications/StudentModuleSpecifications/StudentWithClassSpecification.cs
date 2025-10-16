using School.Application.Common.Enums;
using School.Application.Common.Models.QueryParams;
using School.Domain.Entities;

namespace School.Infrastructure.Implementation.Specifications.StudentModuleSpecifications
{
    public class StudentWithClassSpecification : BaseSpecification<Student, int>
    {
        public StudentWithClassSpecification(StudentQueryParams queryParams)
            : base(s =>
                       (!queryParams.ClassId.HasValue || s.ClassId == queryParams.ClassId)
                    && (string.IsNullOrWhiteSpace(queryParams.SearchTerm) || s.Name.ToLower().Contains(queryParams.SearchTerm.ToLower())))
        {
            AddInclude(x => x.Class!);

            switch (queryParams.SortingOption)
            {
                case SortingOptions.NameAsc:
                    AddOrderBy(P => P.Name);
                    break;
                case SortingOptions.NameDesc:
                    AddOrderByDesc(P => P.Name);
                    break;
                default:
                    break;
            }

            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }

        public StudentWithClassSpecification(int id) : base(s => s.Id == id)
        {
            AddInclude(x => x.Class!);
        }
    }
}
