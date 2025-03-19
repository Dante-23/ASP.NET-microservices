using BudgetService.Clients;
using BudgetService.Models;
using Humanizer;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BudgetService.Services {
    public class ManagerHostedService : IHostedService {
        public const int BUDGET_NOT_FOUND = -1;
        private readonly ExpenseDbService _expenseDbService;
        private Dictionary<string, KeyValuePair<long, long>> _budgetAmountDict;
        // Value.first = amount, value.second = maxAmount
        public ManagerHostedService(ExpenseDbService expenseDbService) {
            _expenseDbService = expenseDbService;
            _budgetAmountDict = new Dictionary<string, KeyValuePair<long, long>>();
        }
        public Task StartAsync(CancellationToken cancellationToken) {
            Console.WriteLine("StartAsync");
            FetchAndStoreAllBudgets();
            Console.WriteLine("StartAsync end");
            return Task.CompletedTask;
        }
        private async void FetchAndStoreAllBudgets() {
            List<Expense> expenses = await _expenseDbService.GetAsync();
            expenses.ForEach(expense => {
                Budget budget = expense.Budget;
                if (!_budgetAmountDict.ContainsKey(budget.BudgetName))
                    _budgetAmountDict.Add(budget.BudgetName, new KeyValuePair<long, long>(expense.Amount, budget.MaxAmount));
                else {
                    KeyValuePair<long, long> currValue = _budgetAmountDict[budget.BudgetName];
                    KeyValuePair<long, long> newValue = new KeyValuePair<long, long> (currValue.Key + expense.Amount, currValue.Value);
                    _budgetAmountDict[budget.BudgetName] = newValue;
                }
            });
        }
        public async Task<long> GetMaxAmountGivenBudgetName(long userid, string budgetName) {
            List<Expense> expenses = await _expenseDbService.GetAsync();
            long response = -1;
            expenses.ForEach(expense => {
                if (expense.UserId == userid && expense.Budget.BudgetName == budgetName) {
                    response = expense.Budget.MaxAmount;
                    return;
                }
            });
            return response;
        }
        public bool AddBudget(string budgetName, KeyValuePair<long, long> keyValuePair) {
            if (_budgetAmountDict.ContainsKey(budgetName)) return false;
            _budgetAmountDict.Add(budgetName, keyValuePair);
            return true;
        }
        public bool UpdateBudgetIncreaseAmount(string budgetName, long amountToIncrease) {
            if (!_budgetAmountDict.ContainsKey(budgetName)) return false;
            KeyValuePair<long, long> value = _budgetAmountDict[budgetName];
            KeyValuePair<long, long> newValue = new(value.Key + amountToIncrease, value.Value);
            _budgetAmountDict[budgetName] = newValue;
            return true;
        }
        public long GetAmountFromBudget(string budgetName) {
            if (!_budgetAmountDict.ContainsKey(budgetName)) return -1;
            return _budgetAmountDict[budgetName].Key;
        }
        public bool DeleteBudget(string budgetName) {
            if (!_budgetAmountDict.ContainsKey(budgetName)) return false;
            _budgetAmountDict.Remove(budgetName);
            return true;
        }
        public void PrintBudgetSummary() {
            foreach (var item in _budgetAmountDict) {
                Console.WriteLine(item.Key + " -> (" + item.Value.Key + ", " + item.Value.Value + ")");
            }
        }
        public Task StopAsync(CancellationToken cancellationToken) {
            Console.WriteLine("StopAsync");
            Console.WriteLine("StopAsync end");
            return Task.CompletedTask;
        }
    }
}