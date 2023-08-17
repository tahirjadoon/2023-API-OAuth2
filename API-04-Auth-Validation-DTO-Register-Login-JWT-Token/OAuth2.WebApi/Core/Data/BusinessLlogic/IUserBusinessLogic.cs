using OAuth2.WebApi.Core.Dto;
using OAuth2.WebApi.Core.Entities;

namespace OAuth2.WebApi.Core.Data.BusinessLogic;

public interface IUserBusinessLogic
{
    Task<IEnumerable<AppUser>> GetAppUsersAsync();

    Task<AppUser> GetAppUserAsync(int id);

    Task<AppUser> GetAppUserAsync(string userName);

    Task<AppUser> GetAppUserAsync(Guid guid);

    Task<LoginResponseDto> RegisterAsync(UserRegisterDto registerUser);

    Task<LoginResponseDto> LoginAsync(LoginDto loginInfo);

}
