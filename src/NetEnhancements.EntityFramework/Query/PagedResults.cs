namespace NetEnhancements.EntityFramework.Query
{
    public record PagedResults<T> where T : class
    {
        public long CurrentPage { get; }
        public long TotalItems { get; }
        public long ItemsPerPage { get; }
        public IList<T> Items { get; }

        public PagedResults(IList<T> items, PagedQuery pagedQuery, long totalItems = 0)
        {
            Items = items;
            ItemsPerPage = pagedQuery.PerPage;
            CurrentPage = pagedQuery.PageNumber;
            TotalItems = totalItems;
        }

        public long TotalPages
        {
            get
            {
                var ceil = Math.Ceiling(TotalItems / (double)ItemsPerPage);
                return Math.Max((long)ceil, 0);
            }
        }
    }
}
