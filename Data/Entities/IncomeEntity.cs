namespace Data.Entities;

public class IncomeEntity
{
    public int Id { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;
    
    public decimal Amount { get; set; }
    
    public string Source { get; set; } = null!;
    
    public string UserId { get; set; } = null!;
}