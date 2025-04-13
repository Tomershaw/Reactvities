using System.Text.Json;

// Summary: This file provides extension methods for enhancing HTTP responses with additional metadata.

namespace API.Extensions
{
    /// <summary>
    /// Enhances HTTP responses with additional metadata.
    /// </summary>
    public static class HttpExtensions
    {
        /// <summary>
        /// Adds a custom Pagination header to the HTTP response.
        /// </summary>
        /// <param name="response">The HTTP response to modify.</param>
        /// <param name="currentPage">The current page number.</param>
        /// <param name="itemsPerPage">The number of items per page.</param>
        /// <param name="totalItems">The total number of items.</param>
        /// <param name="totalPages">The total number of pages.</param>
        public static void AddPaginationHeader(this HttpResponse response, int currentPage,
            int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new
            {
                currentPage,
                itemsPerPage,
                totalItems,
                totalPages
            };

            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader));

            // Note: To expose this header to client apps, use:
            // response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}

// Summary: This file provides extension methods for enhancing HTTP responses with additional metadata.
