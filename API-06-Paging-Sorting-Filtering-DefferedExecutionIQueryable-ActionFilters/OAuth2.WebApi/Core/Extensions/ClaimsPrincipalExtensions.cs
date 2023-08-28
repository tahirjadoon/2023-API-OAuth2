using System.Security.Claims;
using OAuth2.WebApi.Core.Dto;

namespace OAuth2.WebApi.Core.Extensions;

/// <summary>
/// https://www.jerriepelser.com/blog/useful-claimsprincipal-extension-methods/
/// </summary>
public static class ClaimsPrincipalExtensions
{
    public static string GetUserEmail(this ClaimsPrincipal principal)
    {
        if (principal == null) return string.Empty;
        //return principal.FindFirstValue(ClaimTypes.Email);
        var email = principal.FindFirst(ClaimTypes.Email)?.Value;
        return email;
    }

    public static string GetUserName(this ClaimsPrincipal principal)
    {
        if (principal == null) return string.Empty;
        //return principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var userName = principal.FindFirst(ClaimTypes.Name)?.Value;
        return userName;
    }

    public static int GetUserId(this ClaimsPrincipal principal)
    {
        if (principal == null) return 0;
        //return principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        return userId;
    }

    public static string GetDisplayName(this ClaimsPrincipal principal)
    {
        if (principal == null) return string.Empty;
        var displayName = principal.FindFirst("displayName")?.Value;
        return displayName;
    }

    public static Guid GetUserGuid(this ClaimsPrincipal principal)
    {
        var getGuid = Guid.Empty;
        if (principal == null) return getGuid;
        var guid = principal.FindFirst("guid")?.Value;
        if (string.IsNullOrWhiteSpace(guid)) return getGuid;
        try
        {
            getGuid = new Guid(guid);
        }
        catch { }
        return getGuid;
    }

    public static UserClaimsReadDto GetUserClaims(this ClaimsPrincipal principal)
    {
        if (principal == null) return null;
        var claimsDto = new UserClaimsReadDto()
        {
            UserId = principal.GetUserId(),
            UserName = principal.GetUserName(),
            Guid = principal.GetUserGuid(),
            DisplayName = principal.GetDisplayName()
        };
        return claimsDto;
    }
}
