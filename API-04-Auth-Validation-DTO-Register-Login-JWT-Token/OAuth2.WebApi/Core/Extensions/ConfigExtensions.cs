using OAuth2.WebApi.Core.Constants;

namespace OAuth2.WebApi.Core.Extensions;

public static class ConfigExtensions
{
    /* Generic GetSectionValue methods start */
    public static T GetSectionValue<T>(this IConfiguration config, string sectionName, T defaultValue)
    {
        if (!config.GetSection(sectionName).Exists())
        {
            return defaultValue;
        }

        var sValue = config.GetSection(sectionName).Get<T>();
        return sValue;
    }
    /* Generic GetSectionValue methods end */

    /* Helper methods to get the common items using the above GetSectionValue */
    public static string GetDefaultConnectionString(this IConfiguration config)
    {
        var connectionString = config.GetConnectionString(ConfigKeyConstants.DefaultConnection);
        return connectionString;
    }

    public static List<string> GetAllowSpecificOrigins(this IConfiguration config)
    {
        var allowSpecificOrigins = config.GetSectionValue<List<string>>(ConfigKeyConstants.AllowSpecificOrigins, null);
        return allowSpecificOrigins;
    }

    public static string GetLoggingLevelDefault(this IConfiguration config)
    {
        var loggingLevelDefault = config.GetSectionValue<string>(ConfigKeyConstants.LoggingLevelDefault, string.Empty);
        return loggingLevelDefault;
    }

    public static string GetLoggingLevelMsApnetCore(this IConfiguration config)
    {
        var loggingLevelDefault = config.GetSectionValue<string>(ConfigKeyConstants.LoggingLevelMsAspNetCore, string.Empty);
        return loggingLevelDefault;
    }

    public static string GetTokenKey(this IConfiguration config)
    {
        var tokenKey = config.GetSectionValue<string>(ConfigKeyConstants.TokenKey, string.Empty);
        return tokenKey;
    }
}
