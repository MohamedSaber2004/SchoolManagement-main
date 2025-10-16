using School.Application.Common.Enums;

namespace School.Application.Common.Models.QueryParams
{
    public class StudentQueryParams: BaseQueryParams
    {
        public int? ClassId { get; set; }
        public string? SearchTerm { get; set; }
    }
}
