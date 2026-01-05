namespace Data.Entities;

public class SavingHistoryEntity
{
    public int Id { get; set; }
    
    public int SavingId { get; set; }
    
    public SavingEntity Saving { get; set; } = null!;
    
    public decimal Amount { get; set; }
    
    public DateTime Date { get; set; }

    public string Type { get; set; } = "";
}