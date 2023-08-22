using Microsoft.AspNetCore.Mvc;
using OAuth2.WebApi.Core.Data.BusinessLogic;
using OAuth2.WebApi.Core.Dto;
using OAuth2.WebApi.Core.Entities;

namespace OAuth2.WebApi.Controllers;

public class AccountController : BaseApiController
{
    private readonly IUserBusinessLogic _userBL;

    public AccountController(IUserBusinessLogic userBL)
    {
        _userBL = userBL;
    }

    /// <summary>
    /// /api/acount/register
    /// </summary>
    /// <param name="registerUser"></param>
    /// <returns></returns>
    [HttpPost("register", Name = "RegisterUser")] // api/account/register
    public async Task<ActionResult<LoginResponseDto>> Register([FromBody] UserRegisterDto registerUser)
    {
        var user = await _userBL.RegisterAsync(registerUser);
        if (user == null || string.IsNullOrWhiteSpace(user.UserName))
            return BadRequest("Unable to Create User");

        return Ok(user);
    }

    /// <summary>
    /// /api/account/login
    /// </summary>
    /// <param name="loginInfo"></param>
    /// <returns></returns>
    [HttpPost("login", Name = "LoginUser")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto loginInfo)
    {
        var user = await _userBL.LoginAsync(loginInfo);

        if (user == null || string.IsNullOrWhiteSpace(user.UserName))
            return Unauthorized("Unable to login user");

        return Ok(user);
    }

}
