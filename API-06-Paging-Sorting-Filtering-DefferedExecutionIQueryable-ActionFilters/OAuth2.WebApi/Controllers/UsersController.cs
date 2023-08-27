﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OAuth2.WebApi.Core.Data.BusinessLogic;
using OAuth2.WebApi.Core.Dto;
using OAuth2.WebApi.Core.Entities;

namespace OAuth2.WebApi.Controllers;

public class UsersController : BaseApiController
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
    [Authorize]
    [HttpGet]
    //public ActionResult<IEnumerable<AppUser>> GetUsers()
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await _userBL.GetUsersAsync();
        if (users == null || !users.Any())
            return NotFound("No users found!");
        return Ok(users);
    }

    /// <summary>
    /// /api/users/2
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
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
    [Authorize]
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
    [Authorize]
    [HttpGet("guid/{guid}", Name = "GetUserByGuid")]
    public async Task<ActionResult<UserDto>> GetUser(Guid guid)
    {
        var user = await _userBL.GetUserAsync(guid);
        if (user == null)
            return NotFound($"No user found by id {guid}");
        return Ok(user);
    }
}
