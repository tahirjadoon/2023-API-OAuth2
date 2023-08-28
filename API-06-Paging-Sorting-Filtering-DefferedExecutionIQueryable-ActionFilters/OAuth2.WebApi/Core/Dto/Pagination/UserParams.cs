using OAuth2.WebApi.Core.Constants;

namespace OAuth2.WebApi.Core.Dto.Pagination;

/// <summary>
/// user filtering parameters
/// </summary>
public class UserParams : PaginationParams
{
    public Guid? CurrentUserGuid { get; set; }
    public string Gender { get; set; }

    /// <summary>
    /// youngest
    /// </summary>
    public int MinAge { get; set; } = DataConstants.MinAge;

    /// <summary>
    /// oldest
    /// </summary>
    public int MaxAge { get; set; } = DataConstants.MaxAge;

    public string OrderBy { get; set; } = DataConstants.LastActive;
}
