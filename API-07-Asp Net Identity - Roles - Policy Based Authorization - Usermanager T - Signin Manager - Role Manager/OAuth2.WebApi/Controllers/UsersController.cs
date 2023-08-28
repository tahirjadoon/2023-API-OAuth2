using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OAuth2.WebApi.Core.Data.BusinessLogic;
using OAuth2.WebApi.Core.Dto;
using OAuth2.WebApi.Core.Dto.Pagination;
using OAuth2.WebApi.Core.Entities;
using OAuth2.WebApi.Core.Extensions;

namespace OAuth2.WebApi.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserBusinessLogic _userBL;

    public UsersController(IUserBusinessLogic userBL)
    {
        _userBL = userBL;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userParams"></param>
    /// <returns></returns>
    [HttpGet]
    //public ActionResult<IEnumerable<AppUser>> GetUsers()
    //public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    public async Task<ActionResult<PagedList<UserDto>>> GetUsers([FromQuery] UserParams userParams)
    {
        //get the logged in users claims
        //User has the logged in users info
        var userName = User.GetUserName();

        //get the current user from the db
        var currentUser = await _userBL.GetUserAsync(userName);
        if (currentUser == null)
            return BadRequest("User issue");

        //filter the current user
        userParams.CurrentUserGuid = currentUser.Guid;

        //if gender is not supplied then pick the opposite gender of the logged in user 
        if (string.IsNullOrWhiteSpace(userParams.Gender))
            userParams.Gender = currentUser.Gender.ToLowerInvariant() == "male" ? "female" : "male";

        //get the users
        var users = await _userBL.GetUsersAsync(userParams);
        if (users == null || !users.Any())
            return NotFound("No users found!");

        //users has the pagination information so will need to write the pagination header using the extension we created
        Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

        return Ok(users);
    }

    /// <summary>
    /// /api/users/2
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "GetUserById")]
    //public ActionResult<AppUser> GetUser(int id)
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _userBL.GetUserAsync(id);
        if (user == null)
            return NotFound($"No user found by id {id}");
        return Ok(user);
    }

    /// <summary>
    /// /api/users/name/Bob
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [HttpGet("name/{name}", Name = "GetUserByName")]
    public async Task<ActionResult<UserDto>> GetUser(string name)
    {
        var user = await _userBL.GetUserAsync(name);
        if (user == null)
            return NotFound($"No user found by name {name}");
        return Ok(user);
    }

    /// <summary>
    /// /api/users/guid/1870F084-8E81-44FC-AC6A-8640697F5B1A
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    [HttpGet("guid/{guid}", Name = "GetUserByGuid")]
    public async Task<ActionResult<UserDto>> GetUser(Guid guid)
    {
        var user = await _userBL.GetUserAsync(guid);
        if (user == null)
            return NotFound($"No user found by id {guid}");
        return Ok(user);
    }
}
