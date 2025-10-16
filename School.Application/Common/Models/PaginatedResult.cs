namespace School.Application.Common.Models
{
    public class PaginatedResult<TEntity>
    {
        public PaginatedResult(int pageIndex, int pageSize, int totalCount, IEnumerable<TEntity> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            Data = data;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int ItemsCount => Data.Count();
        public int TotalCount { get; set; }
        public IEnumerable<TEntity> Data { get; set; } = [];
    }
}
