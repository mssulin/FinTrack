namespace WebApp.ViewModels;

public class SavingHistoryViewModel
{
    public int Id { get; set; }
    
    public int SavingId { get; set; }
    
    public decimal Amount { get; set; }
    
    public DateTime Date { get; set; }

    public string Type { get; set; } = "";
}