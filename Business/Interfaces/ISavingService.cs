using Data.Entities;

namespace Business.Interfaces;

public interface ISavingService
{
    Task<SavingEntity> AddSavingAsync(SavingEntity saving);
    
    Task<IEnumerable<SavingEntity>> GetAllSavingsAsync(string userId);

    public Task<decimal> GetTotalSavedInPeriodAsync(string userId, DateTime start, DateTime endInclusive);

    Task<SavingEntity?> GetSavingByIdAsync(int id);
    
    Task UpdateSavingAsync(SavingEntity saving);
    
    Task DeleteSavingAsync(SavingEntity saving);
    
    Task AddDepositAsync(int savingId, decimal amount);
    
    Task AddSavingHistoryAsync(SavingHistoryEntity history);
    Task<IEnumerable<SavingHistoryEntity>> GetSavingHistoryAsync(string userId);
    
    Task<IEnumerable<SavingHistoryEntity>> GetSavingHistoryBySavingIdAsync(int savingId);
}