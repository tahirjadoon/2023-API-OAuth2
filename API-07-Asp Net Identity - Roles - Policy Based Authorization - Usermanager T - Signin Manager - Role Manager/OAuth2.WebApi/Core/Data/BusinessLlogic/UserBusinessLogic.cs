using System.ComponentModel.DataAnnotations;
using AutoMapper;
using OAuth2.WebApi.Core.Data.Repositories;
using OAuth2.WebApi.Core.Dto;
using OAuth2.WebApi.Core.Dto.Pagination;
using OAuth2.WebApi.Core.Entities;
using OAuth2.WebApi.Core.ExceptionCustom;
using OAuth2.WebApi.Core.Extensions;
using OAuth2.WebApi.Core.Services;

namespace OAuth2.WebApi.Core.Data.BusinessLogic;

public class UserBusinessLogic : IUserBusinessLogic
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public UserBusinessLogic(IUserRepository userRepository, ITokenService tokenService, IMapper mapper)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<AppUser> GetAppUserAsync(string userName, bool includePhotos = false)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new ValidationException("Invalid userName");

        var appUser = await _userRepository.GetAppUserAsync(userName, includePhotos);
        return appUser;
    }

    /*
    public async Task<IEnumerable<UserDto>> GetUsersAsync()
    {
        var appUsers = await _userRepository.GetUsersAsync();
        if (appUsers == null || !appUsers.Any())
            return null;
        //var users = _mapper.Map<IEnumerable<UserDto>>(appUsers);
        return appUsers;
    }
    */
    public async Task<PagedList<UserDto>> GetUsersAsync(UserParams userParams)
    {
        var users = await _userRepository.GetUsersAsync(userParams);
        if (users == null || !users.Any()) return null;
        return users;
    }

    public async Task<UserDto> GetUserAsync(int id)
    {
        var appUser = await _userRepository.GetUserAsync(id);
        if (appUser == null)
            return null;
        //var user = _mapper.Map<UserDto>(appUser);
        return appUser;
    }

    public async Task<UserDto> GetUserAsync(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new ValidationException("Invalid userName");

        var appUser = await _userRepository.GetUserAsync(userName);
        if (appUser == null)
            return null;
        //var user = _mapper.Map<UserDto>(appUser);
        return appUser;
    }

    public async Task<UserDto> GetUserAsync(Guid guid)
    {
        var appUser = await _userRepository.GetUserAsync(guid);
        if (appUser == null)
            return null;
        //var user = _mapper.Map<UserDto>(appUser);
        return appUser;
    }

    public async Task<LoginResponseDto> RegisterAsync(UserRegisterDto registerUser)
    {
        if (registerUser == null || string.IsNullOrWhiteSpace(registerUser.UserName) || string.IsNullOrWhiteSpace(registerUser.Password))
            throw new ValidationException("User info missing");

        //check user not already taken
        var isUser = await _userRepository.UserExistsAsync(registerUser.UserName);
        if (isUser)
            throw new ValidationException("Username already taken");

        //hash the password using the CryptoExtension. It will give back hash and the Salt
        var passwordHashKey = registerUser.Password.ComputeHashHmacSha512();
        if (passwordHashKey == null)
            throw new ValidationException("Unable to handle provided password");

        //convert to AppUser to register, ID and GUID will be automatically input by EF
        var appUser = new AppUser()
        {
            UserName = registerUser.UserName.ToLower(), //store as lower case always
            PasswordHash = passwordHashKey.Hash,
            PasswordSalt = passwordHashKey.Salt
        };

        //Register user
        var isRegister = await _userRepository.RegisterAsync(appUser);
        if (!isRegister)
            throw new DataFailException("User not registerd");

        //get user from the DB
        var returnUser = await _userRepository.GetAppUserAsync(registerUser.UserName);
        if (returnUser == null)
            throw new DataFailException("Something went wrong. No user found!");

        var loginResponse = BuildLoginResponse(returnUser);

        return loginResponse;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto loginInfo)
    {
        if (loginInfo == null)
            throw new ValidationException("Login info missing");

        var appUser = await _userRepository.GetAppUserAsync(loginInfo.UserName, includePhotos: true);
        if (appUser == null || appUser.PasswordSalt == null || appUser.PasswordHash == null)
            throw new UnauthorizedAccessException("Either username or password is wrong");

        //password is hashed in db. Hash login password and check against the DB one
        var passwordHashKey = loginInfo.Password.ComputeHashHmacSha512(appUser.PasswordSalt);
        if (passwordHashKey == null)
            throw new UnauthorizedAccessException("Either username or password is wrong");

        //both are byte[]
        if (!passwordHashKey.Hash.AreEqual(appUser.PasswordHash))
            throw new UnauthorizedAccessException("Either username or password is wrong");

        var loginResponse = BuildLoginResponse(appUser);

        return loginResponse;
    }

    private LoginResponseDto BuildLoginResponse(AppUser appUser)
    {
        var loginResponse = new LoginResponseDto()
        {
            UserName = appUser.UserName,
            Guid = appUser.Guid,
            Token = _tokenService.CreateToken(appUser),
            Gender = appUser.Gender,
            DisplayName = appUser.DisplayName
        };
        return loginResponse;
    }

    /// <summary>
    /// used by the LogUserAcitivty Action Filter
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public async Task LogUserActivityAsync(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName)) return;

        //app user 
        var user = await _userRepository.GetAppUserAsync(userName);
        if (user == null) return;

        //update the last active date 
        user.LastActive = DateTime.UtcNow;

        //update 
        await _userRepository.SaveAllAsync();
    }
}
