namespace BudgetService.Models;

public class ExpenseDatabaseSettings {
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string CollectionName { get; set; } = null!;
    public string TempConnectionString { get; set; } = null!;
}