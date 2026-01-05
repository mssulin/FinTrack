namespace WebApp.ViewModels;

public class SubscriptionViewModel
{
    
    public int Id { get; set; }
    public IFormFile? Image { get; set; }
    
    public string? ImageUrl { get; set; }
    
    public string Name { get; set; } = null!;
    
    public decimal Amount { get; set; }
    
    public string Category { get; set; } = null!;

    public string Frequency { get; set; } = null!;
    public DateTime LastPaymentDate { get; set; }

    public DateTime NextPaymentDate { get; set; }
    
    public int DaysLeft => (NextPaymentDate - DateTime.Now).Days;
    
    public bool IsPaid { get; set; }
}