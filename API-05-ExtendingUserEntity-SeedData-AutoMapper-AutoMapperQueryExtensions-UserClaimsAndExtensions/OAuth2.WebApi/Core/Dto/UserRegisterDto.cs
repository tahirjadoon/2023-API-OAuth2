using System.ComponentModel.DataAnnotations;

namespace OAuth2.WebApi.Core.Dto;

public class UserRegisterDto
{
    [Required(ErrorMessage = "UserName is empty")]
    [MinLength(5, ErrorMessage = "UserName length must be atleast 5 chars")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is empty")]
    [StringLength(10, MinimumLength = 4)]
    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).+$", ErrorMessage = "Password must have an upper case, a lower case and a number")]
    public string Password { get; set; }
}
