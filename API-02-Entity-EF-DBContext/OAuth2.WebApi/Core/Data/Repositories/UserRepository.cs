using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using OAuth2.WebApi.Core.DB;
using OAuth2.WebApi.Core.Entities;

namespace OAuth2.WebApi.Core.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AppUser>> GetAppUsersAsync()
    {
        //var users = _context.Users.ToList();
        var users = await _context.Users.ToArrayAsync();
        return users;
    }

    public async Task<AppUser> GetAppUserAsync(int id)
    {
        //var user = _context.Users.Find(id);
        var user = await _context.Users.FindAsync(id);
        return user;
    }

    public async Task<AppUser> GetAppUserAsync(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new ValidationException("Invalid userName");

        var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName.ToLower() == userName.ToLower());
        return user;
    }

    public async Task<AppUser> GetAppUserAsync(Guid guid)
    {
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Guid == guid);
        return user;
    }
}
