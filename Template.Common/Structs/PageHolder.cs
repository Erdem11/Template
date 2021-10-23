namespace Template.Common.Structs
{
    public class PageHolder
    {
        private PageHolder(int pageNo, int pageSize, int totalCount = -1)
        {
            PageNo = pageNo;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public int PageNo { get; }
        public int PageSize { get; }
        public int TotalCount { set; get; }
        public static PageHolder Create(int pageNo, int pageSize)
        {
            return new(pageNo, pageSize);
        }
    }
}