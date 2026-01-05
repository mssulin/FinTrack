namespace Data.Entities;

public class SavingEntity
{
    public int Id { get; set; }
    
    public string UserId { get; set; } = null!;
    
    public string Title { get; set; } = null!;
    
    public decimal TargetAmount { get; set; }
    
    public decimal CurrentAmount { get; set; }
    
    public decimal Monthly  { get; set; }
    
    public DateTime Created { get; set; } = DateTime.UtcNow;
    
    public ICollection<SavingHistoryEntity> SavingHistory { get; set; } = new List<SavingHistoryEntity>();
}