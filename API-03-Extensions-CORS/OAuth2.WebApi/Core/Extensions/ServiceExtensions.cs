using Microsoft.EntityFrameworkCore;
using OAuth2.WebApi.Core.Data.BusinessLogic;
using OAuth2.WebApi.Core.Data.Repositories;
using OAuth2.WebApi.Core.DB;

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

}
