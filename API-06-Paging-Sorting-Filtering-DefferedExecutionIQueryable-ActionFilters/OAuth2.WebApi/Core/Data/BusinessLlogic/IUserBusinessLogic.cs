using OAuth2.WebApi.Core.Dto;
using OAuth2.WebApi.Core.Dto.Pagination;
using OAuth2.WebApi.Core.Entities;

namespace OAuth2.WebApi.Core.Data.BusinessLogic;

public interface IUserBusinessLogic
{
    //Task<IEnumerable<UserDto>> GetUsersAsync();
    Task<PagedList<UserDto>> GetUsersAsync(UserParams userParams);

    Task<UserDto> GetUserAsync(int id);

    Task<UserDto> GetUserAsync(string userName);

    Task<UserDto> GetUserAsync(Guid guid);

    Task<LoginResponseDto> RegisterAsync(UserRegisterDto registerUser);

    Task<LoginResponseDto> LoginAsync(LoginDto loginInfo);

    Task<AppUser> GetAppUserAsync(string userName, bool includePhotos = false);

    /// <summary>
    /// used by the LogUserAcitivty Action Filter
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    Task LogUserActivityAsync(string userName);
}
