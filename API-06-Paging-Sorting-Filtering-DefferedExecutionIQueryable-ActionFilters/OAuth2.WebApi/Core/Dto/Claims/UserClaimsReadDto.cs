namespace OAuth2.WebApi.Core.Dto;

public class UserClaimsReadDto
{
    public string UserName { get; set; }
    public int UserId { get; set; }
    public Guid? Guid { get; set; }
    public string DisplayName { get; set; }

    public bool HasUserName => !string.IsNullOrWhiteSpace(UserName);
    public bool HasGuid => Guid != System.Guid.Empty;
}
