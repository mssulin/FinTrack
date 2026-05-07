using Business.Interfaces;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class SavingService(ISavingRepository savingRepository, ISavingHistoryRepository savingHistoryRepository) : ISavingService

{
    private readonly ISavingRepository _savingRepository = savingRepository;
    private readonly ISavingHistoryRepository _savingHistoryRepository = savingHistoryRepository;

    public async Task<SavingEntity> CreateSavingGoalAsync(SavingEntity saving)
    {
        return await _savingRepository.AddAsync(saving);
    }
    
    // Insättning i sparmål
    public async Task AddToSavingAsync(int savingId, decimal amount)
    {
        var saving = await _savingRepository.GetByIdAsync(savingId);
        if (saving is null) return;

        // Öka nuvarande belopp
        saving.CurrentAmount += amount;
        await _savingRepository.UpdateAsync(saving);

        // Sparar i historiken
        await _savingHistoryRepository.AddAsync(new SavingHistoryEntity
        {
            SavingId = saving.Id,
            Amount = amount,
            Date = DateTime.Now,
            Type = "AddToSaving"
        });
    }

    public async Task<IEnumerable<SavingEntity>> GetAllSavingsAsync(string userId)
    {
        return await _savingRepository.GetAllAsync(userId);
    }
    public async Task<SavingEntity?> GetSavingByIdAsync(int id)
    {
        return await _savingRepository.GetByIdAsync(id);
    }
    
    // Sparhistorik
    public async Task<decimal> GetTotalSavedInPeriodAsync(string userId, DateTime start, DateTime endInclusive)
    {
        var savings = await _savingRepository.GetAllAsync(userId);
        return savings
            .SelectMany(s => s.SavingHistory ?? Enumerable.Empty<SavingHistoryEntity>())
            .Where(h => h.Date >= start && h.Date <= endInclusive)
            .Sum(h => h.Amount);
    }

    public async Task UpdateSavingAsync(SavingEntity saving)
    {
        await _savingRepository.UpdateAsync(saving);
    }

    public async Task AddSavingHistoryAsync(SavingHistoryEntity history)
    {
        await _savingHistoryRepository.AddAsync(history);
    }

    public async Task<IEnumerable<SavingHistoryEntity>> GetSavingHistoryAsync(int savingId)
    {
       var history = await _savingHistoryRepository.GetAllAsync();
       return history.Where(s => s.SavingId == savingId)
           .OrderByDescending(s => s.Date);
    }
    
    
    public async Task<IEnumerable<SavingHistoryEntity>> GetSavingHistoryAsync(string userId)
    {
        return await _savingHistoryRepository.GetAllAsync(userId);
    }

    public async Task<IEnumerable<SavingHistoryEntity>> GetSavingHistoryBySavingIdAsync(int savingId)
    {
        var history = await _savingHistoryRepository.GetAllAsync();
        return history.Where(h => h.SavingId == savingId)
            .OrderByDescending(h => h.Date);
    }
    
    public async Task<SavingEntity?> UpdateSavingAsync(
        int id,
        SavingEntity updatedSaving,
        string userId)
    {
        var existingSaving = await _savingRepository.GetByIdAsync(id);

        if (existingSaving == null || existingSaving.UserId != userId)
            return null;

        existingSaving.Title = updatedSaving.Title;
        existingSaving.CurrentAmount = updatedSaving.CurrentAmount;
        existingSaving.TargetAmount = updatedSaving.TargetAmount;

        await _savingRepository.UpdateAsync(existingSaving);

        return existingSaving;
    }

    public async Task DeleteSavingAsync(SavingEntity saving)
    {
        await _savingRepository.DeleteAsync(saving);
    }
}