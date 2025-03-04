using Microsoft.EntityFrameworkCore;
using UserService.Models;
using JwtAuthentication;

var builder = WebApplication.CreateBuilder(args);
var localMysqlConnString = builder.Configuration.GetConnectionString("LocalMysqlConn");

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<JwtTokenHandler>();
builder.Services.AddSingleton<SignUpValidator>();
builder.Services.AddCustomJwtAuthentication();
/* In memory database for development purposes
builder.Services.AddDbContext<UserDbContext>(opt =>
    opt.UseInMemoryDatabase("UserList"));
*/
builder.Services.AddDbContext<UserDbContext> (options => {
    options.UseMySql(localMysqlConnString, ServerVersion.AutoDetect(localMysqlConnString));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
