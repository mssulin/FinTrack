namespace WebApp.ViewModels;

public class SavingViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public decimal CurrentAmount { get; set; }
    public decimal TargetAmount { get; set; }

    public int ProgressPercent => TargetAmount > 0
        ? (int)Math.Round((CurrentAmount / TargetAmount) * 100)
        : 0;
    
    public List<SavingViewModel> Savings { get; set; } = null!;
}