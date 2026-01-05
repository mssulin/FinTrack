using Data.Entities;

namespace Business.Interfaces;

public interface IIncomeService
{
    Task<IncomeEntity> AddIncomeAsync(IncomeEntity income);
    
    Task<IEnumerable<IncomeEntity>> GetIncomesAsync(string userId);
    
    Task<IncomeEntity?> GetIncomeByIdAsync(int id);
    
    Task UpdateIncomeAsync(IncomeEntity income);
    
    Task DeleteIncomeAsync(IncomeEntity income);
}