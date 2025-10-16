using School.Application.Common.Models.QueryParams;
using School.Domain.Entities;

namespace School.Infrastructure.Implementation.Specifications.ClassModuleSpecifications
{
    public class ClassCountSpecification: BaseSpecification<Class,int>
    {
        public ClassCountSpecification(ClassQueryParams queryParams)
                  : base(x =>
          (string.IsNullOrWhiteSpace(queryParams.SearchTerm) || (x.Name.ToLower().Contains(queryParams.SearchTerm.ToLower()))))
        {
            
        }
    }
}
