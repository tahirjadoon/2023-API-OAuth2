using OAuth2.WebApi.Core.Entities;

namespace OAuth2.WebApi.Core.Services;

public interface ITokenService
{
    string CreateToken(AppUser user);
}
