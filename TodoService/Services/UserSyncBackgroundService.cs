using TodoService.Clients;

namespace TodoService.Services {
    public class UserSyncBackgroundService : IHostedService {
        private readonly TodoDbService _todoDbService;
        private readonly UserServiceClient _userServiceClient;
        public UserSyncBackgroundService(UserServiceClient userServiceClient, TodoDbService todoDbService) {
            _todoDbService = todoDbService;
            _userServiceClient = userServiceClient;
        }
        public Task StartAsync(CancellationToken cancellationToken) {
            Console.WriteLine("StartAsync");
            Console.WriteLine("StartAsync end");
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken) {
            Console.WriteLine("StopAsync");
            Console.WriteLine("StopAsync end");
            return Task.CompletedTask;
        }
    }
}