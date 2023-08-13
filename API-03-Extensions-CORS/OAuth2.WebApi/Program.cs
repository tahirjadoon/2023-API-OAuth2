/*
//ImplicitUsings is enable in OAuth2.WebApi.csproj file so these imports are not needed. 
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
*/

using OAuth2.WebApi.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/*Custom Section Start*/
IConfiguration configuration = builder.Configuration;
builder.Services.RegisterDbContext(configuration);
builder.Services.RegisterServices(configuration);
var myAllowSpecificOrigins = builder.Services.RegisterCors(configuration);
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

//CUSTOM: Start 
//ordering is important here. UseCors before UseAuthentication and MapControllers
//
app.UseCors(myAllowSpecificOrigins);
//CUSTOM: END

app.UseAuthorization();

app.MapControllers();

app.Run();
