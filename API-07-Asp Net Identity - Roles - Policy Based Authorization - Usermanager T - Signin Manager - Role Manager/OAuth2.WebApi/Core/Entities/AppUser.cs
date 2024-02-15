using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using OAuth2.WebApi.Core.Extensions;

namespace OAuth2.WebApi.Core.Entities;

public class AppUser : IdentityUser<int>
{
    /*
    /// <summary>
    /// Due to conventions don't need to put [Key] on it since the property name is convention based. 
    /// </summary>
    Removed since identity is implemented now
    [Key]
    public int Id { get; set; }
    */

    /// <summary>
    /// Auto generation like this is not happening. So check the Core/DB/DataContext.cs for more details
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public Guid Guid { get; set; } = Guid.NewGuid();

    /*
    Removed since identity is implemented now
    [Required]
    public string UserName { get; set; }
    
    [Required]
    public byte[] PasswordHash { get; set; }

    [Required]
    public byte[] PasswordSalt { get; set; }
    */

    [Required]
    public DateOnly DateOfBirth { get; set; }

    [Required]
    public string DisplayName { get; set; }

    [Required]
    public string Gender { get; set; }

    public string Introduction { get; set; }

    public string LookingFor { get; set; }

    public string Interests { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    public string Country { get; set; }

    public List<Photo> Photos { get; set; } = new(); //don't need to do new List<Photo>()

    public DateTime LastActive { get; set; } = DateTime.UtcNow;

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;

    //removed with AutoMapperQueryable Extensions and moved to AutoMapperProfiles
    /*
    //Calculate users age from DateOfBirth using the extension method
    public int GetAge()
    {
        return DateOfBirth.CalculateAge();
    }
    */

    /// <summary>
    /// Added due to identity, acting as a join table
    /// </summary>
    public ICollection<AppUserRole> UserRoles { get; set; }
}