using System.ComponentModel.DataAnnotations;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OAuth2.WebApi.Core.Constants;
using OAuth2.WebApi.Core.DB;
using OAuth2.WebApi.Core.Dto;
using OAuth2.WebApi.Core.Dto.Pagination;
using OAuth2.WebApi.Core.Entities;
using OAuth2.WebApi.Core.Extensions;

namespace OAuth2.WebApi.Core.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public UserRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
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

    public async Task<AppUser> GetAppUserAsync(string userName, bool includePhotos = false)
    {
        if (userName == null)
            throw new ValidationException("Invalid userName");
        AppUser appUser = null;
        if (!includePhotos)
        {
            appUser = await _context.Users
                        .SingleOrDefaultAsync(x => x.UserName.ToLower() == userName.ToLower());
        }
        else
        {
            appUser = await _context.Users
                        .Include(p => p.Photos)
                        .SingleOrDefaultAsync(x => x.UserName.ToLower() == userName.ToLower());
        }
        return appUser;
    }

    /*
    public async Task<IEnumerable<AppUser>> GetAppUsersAsync()
    {
        //var users = _context.Users.ToList();
        var users = await _context.Users
                    .Include(p => p.Photos)
                    .ToListAsync();
        return users;
    }
    */

    //rename above GetAppUsersAsync()
    /*
    public async Task<IEnumerable<UserDto>> GetUsersAsync()
    {
        var users = await _context.Users
                            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                            //.AsSplitQuery()
                            //.AsNoTracking()
                            .ToListAsync();
        return users;
    }
    */

    public async Task<PagedList<UserDto>> GetUsersAsync(UserParams userParams)
    {
        var query = _context.Users.AsQueryable();

        //apply filters
        if (userParams.CurrentUserGuid.HasValue)
            query = query.Where(u => u.Guid != userParams.CurrentUserGuid.Value);
        if (!string.IsNullOrWhiteSpace(userParams.Gender))
            query = query.Where(u => u.Gender == userParams.Gender);

        //age >= 
        var minDob = userParams.MaxAge.CalculateMaxDob();
        //age <=
        var maxDob = userParams.MinAge.CalculateMinDob();
        //apply date of borth filter
        query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

        //order by 
        if (!string.IsNullOrWhiteSpace(userParams.OrderBy))
        {
            //the new switch statement. _ is the default
            query = userParams.OrderBy switch
            {
                DataConstants.Created => query.OrderByDescending(u => u.CreatedOn),
                _ => query.OrderByDescending(u => u.LastActive)
            };
        }

        //projectTo to get the photos 
        var finalQuery = query
                        .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                        .AsNoTracking();

        //page list has the static method that receive the IQueryable so use it and will return the object
        var pageList = await PagedList<UserDto>.CreateAsync(finalQuery, userParams.PageNumber, userParams.PageSize);
        return pageList;
    }

    /*
    public async Task<AppUser> GetAppUserAsync(int id)
    {
        //var user = _context.Users.Find(id);
        var user = await _context.Users
                            .Include(p => p.Photos)
                            .SingleOrDefaultAsync(x => x.Id == id);
        return user;
    }
    */
    //rename above GetAppUserAsync(int id)
    public async Task<UserDto> GetUserAsync(int id)
    {
        var user = await _context.Users
                            .Where(x => x.Id == id)
                            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                            //.AsSplitQuery()
                            //.AsNoTracking()
                            .SingleOrDefaultAsync();
        return user;
    }

    /*
    public async Task<AppUser> GetAppUserAsync(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new ValidationException("Invalid userName");

        var user = await _context.Users
                    .Include(p => p.Photos)
                    .SingleOrDefaultAsync(x => x.UserName == userName.ToLower());
        return user;
    }
    */
    //rename above GetAppUserAsync(string userName)
    public async Task<UserDto> GetUserAsync(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new ValidationException("Invalid userName");

        var user = await _context.Users
                    .Where(x => x.UserName == userName.ToLower())
                    .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                    //.AsSplitQuery()
                    //.AsNoTracking()
                    .SingleOrDefaultAsync();
        return user;
    }

    /*
    public async Task<AppUser> GetAppUserAsync(Guid guid)
    {
        var user = await _context.Users
                    .Include(p => p.Photos)
                    .SingleOrDefaultAsync(x => x.Guid == guid);
        return user;
    }
    */
    //rename above GetAppUserAsync(Guid guid)
    public async Task<UserDto> GetUserAsync(Guid guid)
    {
        var user = await _context.Users
                    .Where(x => x.Guid == guid)
                    .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                    //.AsSplitQuery()
                    //.AsNoTracking()
                    .SingleOrDefaultAsync();
        return user;
    }
}
