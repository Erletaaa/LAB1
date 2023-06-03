namespace Lab1.Models
{
    public class PagedResponse<T>
    {
        public T Item { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; } = 12;
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }

        public PagedResponse(T item, int page, int totalCount)
        {
            Item = item;
            Page = page;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
        }
    }
}
