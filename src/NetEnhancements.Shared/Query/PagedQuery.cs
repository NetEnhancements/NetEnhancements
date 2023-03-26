namespace NetEnhancements.Shared.Query
{
    public class PagedQuery
    {
        public const int DefaultPerPage = 24;

        /// <summary>
        /// The one-based "page" of items to retrieve.
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// The number of items to retrieve, and the number of items multiplied by the page number to skip.
        /// </summary>
        public int PerPage { get; set; } = DefaultPerPage;
    }
}
