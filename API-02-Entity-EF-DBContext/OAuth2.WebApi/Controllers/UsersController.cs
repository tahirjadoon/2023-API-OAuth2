using Microsoft.AspNetCore.Mvc;
using OAuth2.WebApi.Core.Data.BusinessLogic;
using OAuth2.WebApi.Core.Entities;

namespace OAuth2.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")] // /api/users
public class UsersController : ControllerBase
{
    private readonly IUserBusinessLogic _userBL;

    public UsersController(IUserBusinessLogic userBL)
    {
        _userBL = userBL;
    }

    /// <summary>
    /// /api/users
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    //public ActionResult<IEnumerable<AppUser>> GetUsers()
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await _userBL.GetAppUsersAsync();
        if (users == null || !users.Any())
            return NotFound("No users found!");
        return Ok(users);
    }

    /// <summary>
    /// /api/users/2
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "GetUserById")]
    //public ActionResult<AppUser> GetUser(int id)
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        var user = await _userBL.GetAppUserAsync(id);
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
    public async Task<ActionResult<AppUser>> GetUser(string name)
    {
        var user = await _userBL.GetAppUserAsync(name);
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
    public async Task<ActionResult<AppUser>> GetUser(Guid guid)
    {
        var user = await _userBL.GetAppUserAsync(guid);
        if (user == null)
            return NotFound($"No user found by id {guid}");
        return Ok(user);
    }
}
