using Microsoft.AspNetCore.Identity;

namespace OAuth2.WebApi.Core.Entities;

/// <summary>
/// Derive from IdentityRole and make the Id int
/// One to many relationship, 
/// each user can have multiple roles and 
/// each role can have multiple users
/// </summary>
public class AppRole : IdentityRole<int>
{
    public ICollection<AppUserRole> UserRoles { get; set; }
}
