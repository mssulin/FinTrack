namespace WebApp.ViewModels;

public class DashboardViewModel
{
    public List<string> IncomeSources { get; set; } = new();
    
    public IncomeViewModel NewIncome { get; set; } = new();
    public decimal TotalIncome { get; set; }
    
    public decimal AvailableMoney { get; set; }
    
    public decimal TotalExpenses { get; set; }
    
    public List<string> ExpenseCategories { get; set; } = new();

    public List<ExpenseViewModel> Expenses { get; set; } = [];
    
    public List<SubscriptionViewModel> Subscriptions { get; set; } = new();
    
    public List<SavingViewModel> Savings { get; set; } = new();
    
    public string CurrentMonth { get; set; } = string.Empty;
    
    public decimal TotalSubscriptions => Subscriptions?.Sum(s => s.Amount) ?? 0;
    
    public decimal SavingPercent { get; set; }
}