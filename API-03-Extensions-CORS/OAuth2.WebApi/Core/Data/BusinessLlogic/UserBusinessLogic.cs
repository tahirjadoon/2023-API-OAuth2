using System.ComponentModel.DataAnnotations;
using OAuth2.WebApi.Core.Data.Repositories;
using OAuth2.WebApi.Core.Entities;

namespace OAuth2.WebApi.Core.Data.BusinessLogic;

public class UserBusinessLogic : IUserBusinessLogic
{
    private readonly IUserRepository _userRepository;

    public UserBusinessLogic(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<AppUser>> GetAppUsersAsync()
    {
        var users = await _userRepository.GetAppUsersAsync();
        return users;
    }

    public Task<AppUser> GetAppUserAsync(int id)
    {
        var user = _userRepository.GetAppUserAsync(id);
        return user;
    }

    public async Task<AppUser> GetAppUserAsync(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new ValidationException("Invalid userName");

        var user = await _userRepository.GetAppUserAsync(userName);
        return user;

    }

    public async Task<AppUser> GetAppUserAsync(Guid guid)
    {
        var user = await _userRepository.GetAppUserAsync(guid);
        return user;
    }
}
