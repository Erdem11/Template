namespace Template.Contracts.V1
{
    public class PaginationQuery
    {
        public const int MinPageNo = 0;
        public const int MinPageSize = 1;
        public const int MaxPageSize = 100;

        private int? _pageNo;
        private int? _pageSize;

        public int PageNo
        {
            get => _pageNo.GetValueOrDefault();
            set => _pageNo
                = _pageNo == default
                    ? MinPageNo
                    : value < MinPageNo
                        ? MinPageNo
                        : value;
        }

        public int PageSize
        {
            get => _pageSize.GetValueOrDefault();
            set => _pageSize
                = value == default
                    ? MaxPageSize
                    : value < MinPageSize
                        ? MinPageSize
                        : value > MaxPageSize
                            ? MaxPageSize
                            : value;
        }

        public PaginationQuery()
        {
            PageNo = MinPageNo;
            PageSize = MaxPageSize;
        }

        public PaginationQuery(int pageNo, int pageSize = MaxPageSize)
        {
            PageNo = pageNo;
            PageSize = pageSize;
        }
    }
}