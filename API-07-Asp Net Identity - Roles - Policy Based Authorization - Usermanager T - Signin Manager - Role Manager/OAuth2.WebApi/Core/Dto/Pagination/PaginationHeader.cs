namespace OAuth2.WebApi.Core.Dto.Pagination;

/// <summary>
/// Helper method, this will be an object that we will return inside HTTP Response Headers
/// </summary>
public class PaginationHeader
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="currentPage">The current page number</param>
    /// <param name="itemsPerPage">The page size</param>
    /// <param name="totalItems">The total items</param>
    /// <param name="totalPages">The total pages</param>
    public PaginationHeader(int currentPage, int itemsPerPage, int totalItems, int totalPages)
    {
        CurrentPage = currentPage;
        ItemsPerPage = itemsPerPage;
        TotalItems = totalItems;
        TotalPages = totalPages;
    }

    public int CurrentPage { get; private set; }
    public int ItemsPerPage { get; private set; }
    public int TotalItems { get; private set; }
    public int TotalPages { get; private set; }
}
