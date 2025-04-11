namespace Application.Core
{
    // Represents pagination parameters sent from the client.
    // Used to control paging of results in API endpoints.
    
    public class PagingParams
    {
        // Maximum allowed page size to prevent performance issues
        private const int MaxPageSize = 50;

        // The current page number (default is 1)
        public int PageNumber { get; set; } = 1;

        // Backing field for page size
        private int _pageSize = 10;

        // Page size (number of items per page) with a limit of MaxPageSize
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
