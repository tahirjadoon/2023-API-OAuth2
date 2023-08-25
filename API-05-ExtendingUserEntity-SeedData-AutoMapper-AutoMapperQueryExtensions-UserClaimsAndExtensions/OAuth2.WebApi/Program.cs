/*
//ImplicitUsings is enable in OAuth2.WebApi.csproj file so these imports are not needed. 
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
*/

using Microsoft.EntityFrameworkCore;
using OAuth2.WebApi.Core.DB;
using OAuth2.WebApi.Core.Extensions;
using OAuth2.WebApi.Core.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/*Custom Section Start*/
IConfiguration configuration = builder.Configuration;
builder.Services.RegisterDbContext(configuration);
builder.Services.RegisterServices(configuration);
var myAllowSpecificOrigins = builder.Services.RegisterCors(configuration);
builder.Services.RegisterAuthentication(configuration);
/*Custom Section End*/

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//CUSTOM: Middleware Start
app.UseMiddleware<ExceptionMiddleware>();
//CUSTOM: Middleware End

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//CUSTOM: Start 
//ordering is important here. UseCors before UseAuthentication and MapControllers
//
app.UseCors(myAllowSpecificOrigins);
app.UseAuthentication();
//CUSTOM: END

app.UseAuthorization();

app.MapControllers();

//CUSTOM: Seed Data Start
//this will give access to all th services
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    //applies any pending migrations for the context to the the database
    //will create the database if it does not already exist
    //just restarting the application will apply our migrations
    await context.Database.MigrateAsync();
    //seed users
    await Seed.SeedUsers(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during seeding of data");
}
//CUSTOM: Seed Data End

app.Run();
