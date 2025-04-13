using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Core
{
    // Represents a paginated list of items, including metadata about the pagination state.
    // Commonly used to return paginated results from database queries.

    public class PagedList<T> : List<T>
    {
        // Private constructor to initialize the paginated list with items and metadata.
        private PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber; // The current page number.
            TotalPages = (int)Math.Ceiling(count / (double)pageSize); // Total number of pages.
            PageSize = pageSize; // Number of items per page.
            TotalCount = count; // Total number of items across all pages.
            AddRange(items); // Add the items for the current page to the list.
        }

        // The current page number being viewed.
        public int CurrentPage { get; set; }

        // The total number of pages available based on the total item count and page size.
        public int TotalPages { get; set; }

        // The number of items displayed per page.
        public int PageSize { get; set; }

        // The total number of items across all pages.
        public int TotalCount { get; set; }

        // Factory method to create a paginated list asynchronously from a queryable source.
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync(); // Get the total number of items in the source.
            var items = await source
                .Skip((pageNumber - 1) * pageSize) // Skip items from previous pages.
                .Take(pageSize) // Take only the items for the current page.
                .ToListAsync(); // Execute the query and convert the result to a list.

            return new PagedList<T>(items, count, pageNumber, pageSize); // Return the paginated list.
        }
    }
}
