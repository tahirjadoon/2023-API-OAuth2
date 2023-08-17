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

    /// <summary>
    /// Save to DB
    /// </summary>
    /// <returns></returns>
    public async Task<bool> SaveAllAsync()
    {
        //make sure that the changes have been saved
        var isSave = await _context.SaveChangesAsync() > 0;
        return isSave;
    }

    /// <summary>
    /// Marking the entity only that it has been modified. Must call SaveAllAsync to save
    /// </summary>
    /// <param name="appUser"></param>
    /// <exception cref="ValidationException"></exception>
    public void Update(AppUser appUser)
    {
        if (appUser == null)
            throw new ValidationException("Invalid user");

        //ef adds a flag to the entity that it has been modified
        _context.Entry<AppUser>(appUser).State = EntityState.Modified;
    }

    /// <summary>
    /// Checks if the userName is taken or not
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public async Task<bool> UserExistsAsync(string userName)
    {
        var isUser = await _context.Users.AnyAsync(x => x.UserName == userName.ToLower());
        return isUser;
    }

    /// <summary>
    /// Add the new user
    /// </summary>
    /// <param name="appUser"></param>
    /// <returns></returns>
    /// <exception cref="ValidationException"></exception>
    public async Task<bool> RegisterAsync(AppUser appUser)
    {
        if (appUser == null)
            throw new ValidationException("Invalid user");

        _context.Users.Add(appUser);
        var isSave = await SaveAllAsync();
        return isSave;
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

        var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == userName.ToLower());
        return user;
    }

    public async Task<AppUser> GetAppUserAsync(Guid guid)
    {
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Guid == guid);
        return user;
    }


}
