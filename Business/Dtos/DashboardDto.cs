namespace Business.Dtos;

public class DashboardDto
{
    public decimal TotalIncome { get; set; }
    
    public List<string> IncomeSources { get; set; } = new();    
    public decimal TotalExpenses { get; set; }
    
    public decimal AvailableMoney { get; set; }

    public string CurrentMonth { get; set; } = string.Empty;

    public List<ExpenseDto> Expenses { get; set; } = [];

    public List<string> ExpenseCategories { get; set; } = new();
    
    public List<SubscriptionDto> Subscriptions { get; set; } = [];
    
    public List<SavingDto> Savings { get; set; } = [];
    
    public decimal SavingPercent { get; set; }
}