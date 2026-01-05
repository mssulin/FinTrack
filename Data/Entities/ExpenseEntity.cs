namespace Data.Entities;

public class ExpenseEntity
{
    public int Id { get; set; }
    
    public DateTime Date { get; set; } = DateTime.UtcNow;
    
    public decimal Amount { get; set; }
    
    public string Category { get; set; } = null!;
    
    public string UserId { get; set; } = null!;
}