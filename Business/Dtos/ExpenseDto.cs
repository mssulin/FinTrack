namespace Business.Dtos;

public class ExpenseDto
{
    public int Id { get; set; }

    public string Category { get; set; } = "";
    
    public decimal Amount { get; set; }
}