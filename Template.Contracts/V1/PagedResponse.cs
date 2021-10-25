using System.Collections.Generic;

namespace Template.Contracts.V1
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public PagedResponse() {}

        public PagedResponse(IEnumerable<T> data)
        {
            Data = data;
        }

        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
    }
}