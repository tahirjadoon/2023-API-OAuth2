/*
//ImplicitUsings is enable in OAuth2.WebApi.csproj file so these imports are not needed. 
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
*/

using Microsoft.EntityFrameworkCore;
using OAuth2.WebApi.Core.Constants;
using OAuth2.WebApi.Core.Data.BusinessLogic;
using OAuth2.WebApi.Core.Data.Repositories;
using OAuth2.WebApi.Core.DB;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/*Custom Section Start*/
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString(ConfigKeyConstants.DefaultConnection));
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserBusinessLogic, UserBusinessLogic>();
/*Custom Section End*/

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
