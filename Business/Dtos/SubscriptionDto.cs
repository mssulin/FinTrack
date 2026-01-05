namespace Business.Dtos;

public class SubscriptionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    
    public string? ImageUrl { get; set; }
    
    public decimal Amount { get; set; }
    
    public string Category { get; set; } = "";
    
    public string Frequency { get; set; } = "";
    
    public DateTime LastPaymentDate { get; set; }
    
    public DateTime NextPaymentDate { get; set; }
    
    public bool IsPaid { get; set; }
}