namespace OAuth2.WebApi.Core.Dto;

public class LoginResponseDto
{
    public string UserName { get; set; }

    public Guid Guid { get; set; }

    public string Token { get; set; }
}
