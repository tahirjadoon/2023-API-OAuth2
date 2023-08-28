using OAuth2.WebApi.Core.Constants;
using OAuth2.WebApi.Core.Dto.Pagination;

namespace OAuth2.WebApi.Core.Extensions;

public static class HttpExtensions
{
    /// <summary>
    /// add pagination header onto the response
    /// </summary>
    /// <param name="response"></param>
    /// <param name="currentPage"></param>
    /// <param name="itemsPerPage"></param>
    /// <param name="totalItems"></param>
    /// <param name="totalPages"></param>
    public static void AddPaginationHeader(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
    {
        var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
        response.AddPaginationHeader(paginationHeader);
    }

    /// <summary>
    /// add pagination header onto the response
    /// </summary>
    /// <param name="response"></param>
    /// <param name="paginationHeader"></param>
    public static void AddPaginationHeader(this HttpResponse response, PaginationHeader paginationHeader)
    {
        var paginationHeaderSerialize = paginationHeader.ToJson();

        //write custom header. No more adding X- to it. Give a sensible name, following will put "Pagination"
        var headerName = HeaderNameConstants.Pagination;

        //add header
        response.Headers.Add(headerName, paginationHeaderSerialize);

        //need to add the CORS header as well since a custom header is being used to make it available
        //cors header must be specific name
        response.Headers.Add(HeaderNameConstants.AccessControlExposeHeaders, headerName);
    }
}
