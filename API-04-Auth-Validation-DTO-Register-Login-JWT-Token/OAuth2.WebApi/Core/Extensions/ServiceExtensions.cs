using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OAuth2.WebApi.Core.Data.BusinessLogic;
using OAuth2.WebApi.Core.Data.Repositories;
using OAuth2.WebApi.Core.DB;
using OAuth2.WebApi.Core.Services;

namespace OAuth2.WebApi.Core.Extensions;

public static class ServiceExtensions
{

    /// <summary>
    /// Register DbContext
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlite(configuration.GetDefaultConnectionString());
        });
    }

    /// <summary>
    /// Register Services like DI etc
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserBusinessLogic, UserBusinessLogic>();

        services.AddScoped<ITokenService, TokenService>();
    }

    /// <summary>
    /// Register CORS as policy
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static string RegisterCors(this IServiceCollection services, IConfiguration configuration)
    {
        var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
        //https://stackoverflow.com/questions/42858335/how-to-hardcode-and-read-a-string-array-in-appsettings-json
        var allowedSpecificOrigins = configuration.GetAllowSpecificOrigins();
        if (allowedSpecificOrigins != null && allowedSpecificOrigins.Any())
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: myAllowSpecificOrigins,
                                policy =>
                                {
                                    policy.WithOrigins(allowedSpecificOrigins.ToArray())
                                    .AllowAnyHeader()
                                    .AllowAnyMethod();
                                });
            });
        }
        return myAllowSpecificOrigins;
    }

    /// <summary>
    /// Register authentications service
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void RegisterAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        //get the token from config and change to byte array
        var tokenKey = Encoding.UTF8.GetBytes(configuration.GetTokenKey());

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
                        ValidateIssuer = false, //issuer of the token
                        ValidateAudience = false
                    };
                });
    }

}
