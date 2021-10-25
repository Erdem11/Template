namespace Template.Common.Structs
{
    public class PaginationFilter
    {
        public int PageNo { set; get; }
        public int PageSize { set; get; }
        public int TotalCount { set; get; }
        public PaginationFilter()
        {

        }

        private PaginationFilter(int pageNo, int pageSize, int totalCount = -1)
        {
            PageNo = pageNo;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public static PaginationFilter Create(int pageNo, int pageSize, int totalCount = -1)
        {
            return new PaginationFilter(pageNo, pageSize, totalCount);
        }
    }
}