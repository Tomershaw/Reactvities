namespace Application.Core
{
    // Represents the parameters used for pagination in API requests.
    // Helps control the number of items returned and the specific page of results.

    public class PagingParams
    {
        // The maximum number of items allowed per page to prevent performance issues.
        private const int MaxPageSize = 50;

        // The current page number requested by the client (default is the first page).
        public int PageNumber { get; set; } = 1;

        // Backing field for the page size, used to enforce the maximum limit.
        private int _pageSize = 10;

        // The number of items per page, constrained by the maximum page size.
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
