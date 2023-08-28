namespace OAuth2.WebApi.Core.Dto;

public class PhotoDto
{
    public int Id { get; set; }

    public string URL { get; set; }

    public bool IsMain { get; set; } = false;
}