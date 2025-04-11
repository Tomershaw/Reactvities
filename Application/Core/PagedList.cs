using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Core
{
    // Represents a paginated list of items.
    // Includes metadata such as current page, total pages, and total item count.
    // Commonly used for paging results in API responses.

    public class PagedList<T> : List<T>
    {
        // Constructor that builds the paged list with pagination info and items
        private PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            TotalCount = count;
            AddRange(items);
        }

        // Current page number
        public int CurrentPage { get; set; }

        // Total number of pages (based on total items and page size)
        public int TotalPages { get; set; }

        // Number of items per page
        public int PageSize { get; set; }

        // Total number of items
        public int TotalCount { get; set; }

        // Factory method that executes the query and builds a PagedList<T> asynchronously
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync(); // total number of items
            var items = await source
                .Skip((pageNumber - 1) * pageSize) // skip items from previous pages
                .Take(pageSize)                    // take only the items for the current page
                .ToListAsync();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
