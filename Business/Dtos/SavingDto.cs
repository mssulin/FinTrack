namespace Business.Dtos;

public class SavingDto
{
    public int Id { get; set; }
    
    public string Title { get; set; } = "";
    
    public decimal CurrentAmount { get; set; }
    
    public decimal TargetAmount { get; set; }
}