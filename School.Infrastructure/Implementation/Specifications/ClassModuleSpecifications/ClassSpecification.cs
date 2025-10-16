using School.Application.Common.Enums;
using School.Application.Common.Models.QueryParams;
using School.Domain.Entities;

namespace School.Infrastructure.Implementation.Specifications.ClassModuleSpecifications
{
    public class ClassSpecification: BaseSpecification<Class,int>
    {
        public ClassSpecification(ClassQueryParams queryParams)
            : base(c =>
                       (string.IsNullOrWhiteSpace(queryParams.SearchTerm) || c.Name.ToLower().Contains(queryParams.SearchTerm.ToLower())))
        {
            switch (queryParams.SortingOption)
            {
                case SortingOptions.NameAsc:
                    AddOrderBy(x => x.Name);
                    break;
                case SortingOptions.NameDesc:
                    AddOrderByDesc(x => x.Name);
                    break;
                default:
                    break;
            }

            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }

        public ClassSpecification(int id) : base(s => s.Id == id) { }
    }
}
