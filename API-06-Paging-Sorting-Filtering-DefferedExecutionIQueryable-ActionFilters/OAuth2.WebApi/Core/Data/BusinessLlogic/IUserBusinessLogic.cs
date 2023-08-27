using OAuth2.WebApi.Core.Dto;
using OAuth2.WebApi.Core.Entities;

namespace OAuth2.WebApi.Core.Data.BusinessLogic;

public interface IUserBusinessLogic
{
    Task<IEnumerable<UserDto>> GetUsersAsync();

    Task<UserDto> GetUserAsync(int id);

    Task<UserDto> GetUserAsync(string userName);

    Task<UserDto> GetUserAsync(Guid guid);

    Task<LoginResponseDto> RegisterAsync(UserRegisterDto registerUser);

    Task<LoginResponseDto> LoginAsync(LoginDto loginInfo);

    Task<AppUser> GetAppUserAsync(string userName, bool includePhotos = false);
}
