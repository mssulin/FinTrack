using Data.Entities;

namespace Business.Interfaces;

public interface ISavingService
{
    Task<SavingEntity> CreateSavingGoalAsync(SavingEntity saving);
    
    Task AddToSavingAsync(int savingId, decimal amount);
    
    Task<IEnumerable<SavingEntity>> GetAllSavingsAsync(string userId);

    public Task<decimal> GetTotalSavedInPeriodAsync(string userId, DateTime start, DateTime endInclusive);

    Task<SavingEntity?> GetSavingByIdAsync(int id);
    
    Task UpdateSavingAsync(SavingEntity saving);
    
    Task DeleteSavingAsync(SavingEntity saving);
    
    Task AddSavingHistoryAsync(SavingHistoryEntity history);
    Task<IEnumerable<SavingHistoryEntity>> GetSavingHistoryAsync(string userId);
    
    Task<IEnumerable<SavingHistoryEntity>> GetSavingHistoryBySavingIdAsync(int savingId);
}