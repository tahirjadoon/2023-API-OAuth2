using OAuth2.WebApi.Core.Dto;
using OAuth2.WebApi.Core.Entities;

namespace OAuth2.WebApi.Core.Data.Repositories;

public interface IUserRepository
{
    Task<bool> SaveAllAsync();

    void Update(AppUser appUser);

    Task<bool> UserExistsAsync(string userName);

    Task<bool> RegisterAsync(AppUser appUser);

    Task<AppUser> GetAppUserAsync(string userName, bool includePhotos = false);

    Task<IEnumerable<UserDto>> GetUsersAsync();

    Task<UserDto> GetUserAsync(int id);

    Task<UserDto> GetUserAsync(string userName);

    Task<UserDto> GetUserAsync(Guid guid);
}
