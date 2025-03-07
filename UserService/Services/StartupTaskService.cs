using System;
using System.Threading;
using System.Threading.Tasks;
using JwtAuthentication;
using JwtAuthentication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserService.Models;

namespace UserService.Services;

public class StartupTaskService : IHostedService {
    private readonly JwtTokenHandler _jwtTokenHandler;
    // private readonly UserDbContext _context;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public StartupTaskService(JwtTokenHandler jwtTokenHandler, IServiceScopeFactory serviceScopeFactory) {
        _jwtTokenHandler = jwtTokenHandler;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken) {
        Console.WriteLine("StartAsync");
        // Place your initialization code here
        var scope = _serviceScopeFactory.CreateScope();
        UserDbContext context = scope.ServiceProvider.GetRequiredService<UserDbContext>();

        // Example: Initialize data, configure services, etc.
        LoadUsers(context);
        Console.WriteLine("StartAsync end");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        Console.WriteLine("StopAsync");
        // Optional: Place code here to run when the application is shutting down
        Console.WriteLine("StopAsync end");
        return Task.CompletedTask;
    }

    private async void LoadUsers(UserDbContext context) {
        // Your one-time initialization logic
        var users = await context.Users.ToListAsync();
        users.ForEach(user => {
            _jwtTokenHandler.AddUser(new UserAccount { Id = user.Id, Username = user.Username, Password = user.Password, Role = "User" });
        });
        // Add your initialization code here
    }
}
