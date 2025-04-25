// Represents pagination details for a dataset
export interface Pagination {
  currentPage: number; // Current page number
  itemsPerPage: number; // Number of items per page
  totalItems: number; // Total number of items
  totalPages: number; // Total number of pages
}

// Encapsulates a dataset and its pagination details
export class PaginatedResult<T> {
  data: T; // The dataset
  pagination: Pagination; // Pagination details

  constructor(data: T, pagination: Pagination) {
    this.data = data;
    this.pagination = pagination;
  }
}

// Represents parameters for paging requests
export class PagingParams {
  pageNumber; // Page number to request
  pageSize; // Number of items per page

  constructor(pageNumber = 1, pageSize = 2) {
    this.pageNumber = pageNumber;
    this.pageSize = pageSize;
  }
}
