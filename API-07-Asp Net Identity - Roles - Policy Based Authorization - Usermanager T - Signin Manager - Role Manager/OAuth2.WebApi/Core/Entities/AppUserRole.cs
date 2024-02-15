using Microsoft.AspNetCore.Identity;

namespace OAuth2.WebApi.Core.Entities;

/// <summary>
/// Will join AppUser and AppRole
/// Derive from IdentityUserRole
/// </summary>
public class AppUserRole : IdentityUserRole<int>
{
    public AppUser User { get; set; }

    public AppRole Role { get; set; }
}
