using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OAuth2.WebApi.Core.Entities;
using OAuth2.WebApi.Core.Extensions;

namespace OAuth2.WebApi.Core.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// key will remain the server and it will never go to the client
    /// </summary>
    private readonly SymmetricSecurityKey _key;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;

        //get the TokenKey from the config
        var tokenKey = _configuration.GetTokenKey();
        if (string.IsNullOrWhiteSpace(tokenKey))
            throw new Exception("TokenKey missing");

        //convert tokenKey to bytesArray
        var tokenKeyByteArray = Encoding.UTF8.GetBytes(tokenKey);

        //create key
        _key = new SymmetricSecurityKey(tokenKeyByteArray);
    }

    /// <summary>
    /// Create JWT Token
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public string CreateToken(AppUser user)
    {
        if (user == null)
            throw new Exception("User info missing");

        //claims
        //check folowing url for all available fields. We can create our own as well
        //https://learn.microsoft.com/en-us/dotnet/api/system.identitymodel.tokens.jwt.jwtregisteredclaimnames?view=msal-web-dotnet-latest
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new Claim("guid", user.Guid.ToString()),
            new Claim("displayName", user.DisplayName),
        };

        //signing credentials with key
        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        //describe token
        //check for issuer and audience: https://security.stackexchange.com/questions/256794/should-i-specify-jwt-audience-and-issuer-if-i-have-only-one-spa-client
        //Check extension ServiceExtension.RegisterAuthentication as well
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7), //typically shorter value
            SigningCredentials = credentials
        };

        //token handler
        var tokenHandler = new JwtSecurityTokenHandler();

        //token
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var writeToken = tokenHandler.WriteToken(token);
        return writeToken;
    }
}