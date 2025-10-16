namespace School.Application.Common.Models.QueryParams
{
    public class ChatQueryParams
    {
        private static readonly int _defaultPageSize = 30;
        private static readonly int _maxPageSize = 50;

        public string SenderId { get; set; } = default!;
        public string? ReceiverId { get; set; } = default!;

        public int PageIndex { get; set; } = 1;

        private int pageSize = _defaultPageSize;

        public int PageSize 
        {
            get { return pageSize; }
            set { pageSize = value > _maxPageSize ? _maxPageSize : value; }
        }
    }
}
