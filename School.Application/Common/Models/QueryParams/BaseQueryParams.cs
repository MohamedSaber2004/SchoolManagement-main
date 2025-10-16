using School.Application.Common.Enums;

namespace School.Application.Common.Models.QueryParams
{
    public abstract class BaseQueryParams
    {
        private static readonly int defaultPageSize = 5;
        private static readonly int maxPageSize = 10;

        public SortingOptions SortingOption { get; set; }
        public int PageIndex { get; set; } = 1;

        private int pageSize = defaultPageSize;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > maxPageSize ? maxPageSize : value; }
        }
    }
}
