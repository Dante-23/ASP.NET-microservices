using Microsoft.EntityFrameworkCore;
using TodoService.Models;
using JwtAuthentication;
using TodoService.Clients;
using TodoService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<JwtTokenHandler>();
builder.Services.AddCustomJwtAuthentication();
builder.Services.AddDbContext<TodoDbContext>(opt =>
    opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHostedService<UserSyncBackgroundService>();
builder.Services.AddHttpClient<UserServiceClient>(client => {
    client.BaseAddress = new Uri("http://localhost:5110");
});

builder.Services.Configure<TodoDatabaseSettings>(
    builder.Configuration.GetSection("TodoDatabase"));

builder.Services.AddSingleton<TodoDbService>();
builder.Services.AddCors(options => {
    options.AddPolicy("ReactApp", policyBuilder => {
        policyBuilder.WithOrigins("http://localhost:3001");
        policyBuilder.AllowAnyHeader();
        policyBuilder.AllowAnyMethod();
        policyBuilder.AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.UseCors("ReactApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
