﻿using Microsoft.EntityFrameworkCore;
using OAuth2.WebApi.Core.Entities;

namespace OAuth2.WebApi.Core.DB;

/// <summary>
/// DataContext Class, add as a service to program.cs
/// </summary>
public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<AppUser> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //AppUser has guid that needs to be autogenerated for insert
        modelBuilder.Entity<AppUser>()
        .Property(x => x.Guid)
        .ValueGeneratedOnAdd();
    }
}
