using Microsoft.EntityFrameworkCore;

namespace OAuth2.WebApi.Core.Dto.Pagination;

/// <summary>
/// Pagination Helper Class
/// </summary>
/// <typeparam name="T"></typeparam>
public class PagedList<T> : List<T>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="items">List of items</param>
    /// <param name="count">Total count of items</param>
    /// <param name="pageNumber">Page number interested in</param>
    /// <param name="pageSize">Total records in a page</param>
    public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
        //page number
        CurrentPage = pageNumber;
        //10 items with page size of 4 will end up with 3 pages
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        //page size
        PageSize = pageSize;
        //total count
        TotalCount = count;

        //when we will create a new instance we will return the list of our items
        AddRange(items);
    }

    /// <summary>
    /// Current page number
    /// </summary>
    public int CurrentPage { get; private set; }

    /// <summary>
    /// Total pages by calculating against PageSize and TotalCount
    /// </summary>
    public int TotalPages { get; private set; }

    /// <summary>
    /// Number of records in a page
    /// </summary>
    public int PageSize { get; private set; }

    /// <summary>
    /// Total items
    /// </summary>
    public int TotalCount { get; private set; }

    /// <summary>
    /// Static method so that can call the pagedList and get back the page data
    /// </summary>
    /// <param name="source"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        //execute agains the database and get the total count
        var count = await source.CountAsync();
        //skip the pages to go to the intended page pick the records
        //on page = 1 do not skip any pages
        var items = await source.Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
        //return the data as paged list
        var data = new PagedList<T>(items, count, pageNumber, pageSize);
        return data;
    }
}
