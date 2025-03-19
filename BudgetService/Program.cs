using BudgetService.Clients;
using BudgetService.Models;
using BudgetService.Services;
using JwtAuthentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<JwtTokenHandler>();
builder.Services.AddCustomJwtAuthentication();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpClient<UserServiceClient>(client => {
    client.BaseAddress = new Uri("http://localhost:5110");
});

builder.Services.Configure<ExpenseDatabaseSettings>(
    builder.Configuration.GetSection("BudgetDatabase"));

builder.Services.AddSingleton<ExpenseDbService>();
builder.Services.AddCors(options => {
    options.AddPolicy("ReactApp", policyBuilder => {
        policyBuilder.WithOrigins("http://localhost:3001");
        policyBuilder.AllowAnyHeader();
        policyBuilder.AllowAnyMethod();
        policyBuilder.AllowCredentials();
    });
});

builder.Services.AddSingleton<ManagerHostedService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<ManagerHostedService>());

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
