namespace OAuth2.WebApi.Core.Dto;

public class LoginResponseDto
{
    public string UserName { get; set; }

    public Guid Guid { get; set; }

    public string Token { get; set; }

    public string Gender { get; set; }

    public string DisplayName { get; set; }
}
