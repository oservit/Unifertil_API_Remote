namespace Libs.Common
{

    public class DataPagedResult : DataResult
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public long TotalCount { get; set; }
    }
}
