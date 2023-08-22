namespace OAuth2.WebApi.Core.Dto;

public class HashKeyDto
{
    public byte[] Salt { get; set; }
    public byte[] Hash { get; set; }
}
