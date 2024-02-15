using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using OAuth2.WebApi.Core.Entities;
using OAuth2.WebApi.Core.Extensions;

namespace OAuth2.WebApi.Core.DB;

public class Seed
{
    public static async Task SeedUsers(DataContext context)
    {
        //if we have users in the table then do not do any thing
        if (await context.Users.AnyAsync()) return;

        //seed file location
        var file = "Core/DB/UserSeedData.json";

        //check file exists
        var isFile = await Task.Run(() => File.Exists(file));
        if (!isFile) return;

        //read file
        var userData = await File.ReadAllTextAsync(file);
        //make sure that we have user data
        if (string.IsNullOrWhiteSpace(userData)) return;

        //casing issues in seed data 
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        //get object from json
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

        //check users
        if (users == null || !users.Any()) return;

        //all the users will get the same password so get it here outside the loop
        var hashKey = "Pa$$w0rd1".ComputeHashHmacSha512();
        if (hashKey == null) return;

        //add password to the users, make username lower case and track users
        foreach (var user in users)
        {
            user.UserName = user.UserName.ToLowerInvariant();
            //removed due to Identity
            //user.PasswordHash = hashKey.Hash;
            //user.PasswordSalt = hashKey.Salt;

            //we are only adding tracking to the user, save changes will happen outside of the loop
            context.Users.Add(user);
        }

        //add to the database
        await context.SaveChangesAsync();
    }
}
