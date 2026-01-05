namespace WebApp.ViewModels;

public class ExpenseViewModel
{
    public int Id { get; set; }
    
    public DateTime Date { get; set; }
    
    public decimal Amount { get; set; }

    public string Category { get; set; } = null!;
}