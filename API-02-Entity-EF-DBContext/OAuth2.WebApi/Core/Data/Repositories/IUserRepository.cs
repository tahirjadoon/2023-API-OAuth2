using OAuth2.WebApi.Core.Entities;

namespace OAuth2.WebApi.Core.Data.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<AppUser>> GetAppUsersAsync();

    Task<AppUser> GetAppUserAsync(int id);

    Task<AppUser> GetAppUserAsync(string userName);

    Task<AppUser> GetAppUserAsync(Guid guid);
}
