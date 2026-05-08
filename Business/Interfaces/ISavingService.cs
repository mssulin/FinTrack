using Business.Dtos;
using Data.Entities;

namespace Business.Interfaces;

public interface ISavingService
{
    Task<SavingDto> CreateSavingGoalAsync(SavingDto saving, string userId);

    Task AddToSavingAsync(int savingId, decimal amount);

    Task<IEnumerable<SavingDto>> GetAllSavingsAsync(string userId);

    Task<decimal> GetTotalSavedInPeriodAsync(string userId, DateTime start, DateTime endInclusive);

    Task<SavingDto?> GetSavingByIdAsync(int id);

    Task<SavingDto?> UpdateSavingAsync(
        int id,
        SavingDto updatedSaving,
        string userId);

    Task DeleteSavingAsync(int id, string userId);

    Task<IEnumerable<SavingHistoryEntity>> GetSavingHistoryAsync(string userId);

    Task<IEnumerable<SavingHistoryEntity>> GetSavingHistoryAsync(int savingId);

    Task<IEnumerable<SavingHistoryEntity>> GetSavingHistoryBySavingIdAsync(int savingId);
}