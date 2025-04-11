using System.Text.Json;

namespace API.Extensions
{
    // Extension methods for enhancing HTTP responses with additional metadata
    public static class HttpExtensions
    {
        // Adds a custom Pagination header to the HTTP response
        // This header includes metadata about the current page, total items, etc.
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

            // Add the pagination data as a serialized JSON header
            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader));

            // Note: To make this header visible to client apps (e.g., browsers), 
            // you can expose it via CORS using: 
            // response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
