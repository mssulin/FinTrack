using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class SubscriptionEntity
{
    public int Id { get; set; }
    
    public string? ImageUrl { get; set; }
    
    public string Name { get; set; } = null!;
    
    public decimal Amount { get; set; }
    
    public string Category { get; set; } = null!;
    
    public DateTime LastPaymentDate { get; set; }
    
    public DateTime NextPaymentDate { get; set; }
    
    public string Frequency { get; set; } = null!;
    
    public string UserId { get; set; } = null!;
    
    public bool IsPaid { get; set; } = false;
    
}