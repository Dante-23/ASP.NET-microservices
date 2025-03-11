using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoService.Models;

namespace TodoService.Services {
    public class TodoDbService {
        private readonly IMongoCollection<Todo> _todosCollection;
        public TodoDbService(IOptions<TodoDatabaseSettings> todoDatabaseSettings) {
            var mongoClient = new MongoClient(
            todoDatabaseSettings.Value.TempConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(
            todoDatabaseSettings.Value.DatabaseName);
            _todosCollection = mongoDatabase.GetCollection<Todo>(
            todoDatabaseSettings.Value.CollectionName);
        }

        public async Task<List<Todo>> GetAsync() =>
            await _todosCollection.Find(_ => true).ToListAsync();

        public async Task<Todo?> GetAsync(string id) =>
            await _todosCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<Todo>> GetTodoOfUserId(long userid) =>
            await _todosCollection.Find(x => x.UserId == userid).ToListAsync();
        
        public async Task CreateAsync(Todo newTodo) =>
            await _todosCollection.InsertOneAsync(newTodo);

        public async Task UpdateAsync(string id, Todo updatedTodo) =>
            await _todosCollection.ReplaceOneAsync(x => x.Id == id, updatedTodo);

        public async Task RemoveAsync(string id) =>
            await _todosCollection.DeleteOneAsync(x => x.Id == id);
    }
}