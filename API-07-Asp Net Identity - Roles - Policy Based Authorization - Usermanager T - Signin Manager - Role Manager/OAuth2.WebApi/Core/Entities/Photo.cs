using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OAuth2.WebApi.Core.Entities;

[Table("Photos")]
public class Photo
{
    public int Id { get; set; }

    [Required]
    public string URL { get; set; }

    public bool IsMain { get; set; } = false;

    public string PublicId { get; set; }

    //fully defining the relationship between AppUser and Photos
    //this way an orphan entry will not be created. 
    //if null are allowed then we dont need to do the following
    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}
