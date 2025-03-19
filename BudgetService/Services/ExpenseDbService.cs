using Microsoft.Extensions.Options;
using MongoDB.Driver;
using BudgetService.Models;

namespace BudgetService.Services {
    public class ExpenseDbService {
        private readonly IMongoCollection<Expense> _expensesCollection;
        public ExpenseDbService(IOptions<ExpenseDatabaseSettings> expenseDatabaseSettings) {
            var mongoClient = new MongoClient(
            expenseDatabaseSettings.Value.TempConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(
            expenseDatabaseSettings.Value.DatabaseName);
            _expensesCollection = mongoDatabase.GetCollection<Expense>(
            expenseDatabaseSettings.Value.CollectionName);
        }

        public async Task<List<Expense>> GetAsync() =>
            await _expensesCollection.Find(_ => true).ToListAsync();

        public async Task<Expense?> GetAsync(string id) =>
            await _expensesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<Expense>> GetExpensesOfUserId(long userid) =>
            await _expensesCollection.Find(x => x.UserId == userid).ToListAsync();
        
        public async Task CreateAsync(Expense newExpense) =>
            await _expensesCollection.InsertOneAsync(newExpense);

        public async Task UpdateAsync(string id, Expense updatedExpense) =>
            await _expensesCollection.ReplaceOneAsync(x => x.Id == id, updatedExpense);

        public async Task RemoveAsync(string id) =>
            await _expensesCollection.DeleteOneAsync(x => x.Id == id);
    }
}