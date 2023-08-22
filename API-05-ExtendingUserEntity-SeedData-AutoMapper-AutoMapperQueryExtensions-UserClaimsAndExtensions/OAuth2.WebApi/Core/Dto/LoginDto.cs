using System.ComponentModel.DataAnnotations;

namespace OAuth2.WebApi.Core.Dto;

public class LoginDto
{
    [Required(ErrorMessage = "UserName is empty")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is empty")]
    public string Password { get; set; }
}
