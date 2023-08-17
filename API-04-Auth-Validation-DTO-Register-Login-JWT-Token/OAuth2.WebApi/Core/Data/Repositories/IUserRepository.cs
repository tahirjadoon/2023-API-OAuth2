using OAuth2.WebApi.Core.Entities;

namespace OAuth2.WebApi.Core.Data.Repositories;

public interface IUserRepository
{
    Task<bool> SaveAllAsync();

    void Update(AppUser appUser);

    Task<bool> UserExistsAsync(string userName);

    Task<bool> RegisterAsync(AppUser appUser);

    Task<IEnumerable<AppUser>> GetAppUsersAsync();

    Task<AppUser> GetAppUserAsync(int id);

    Task<AppUser> GetAppUserAsync(string userName);

    Task<AppUser> GetAppUserAsync(Guid guid);
}
