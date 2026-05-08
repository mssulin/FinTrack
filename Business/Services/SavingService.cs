using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class SavingService(
    ISavingRepository savingRepository,
    ISavingHistoryRepository savingHistoryRepository) : ISavingService
{
    private readonly ISavingRepository _savingRepository = savingRepository;
    private readonly ISavingHistoryRepository _savingHistoryRepository = savingHistoryRepository;

    public async Task<SavingDto> CreateSavingGoalAsync(SavingDto saving, string userId)
    {
        var entity = SavingFactory.ToEntity(saving, userId);

        var result = await _savingRepository.AddAsync(entity);

        return SavingFactory.ToDto(result);
    }

    public async Task AddToSavingAsync(int savingId, decimal amount)
    {
        var saving = await _savingRepository.GetByIdAsync(savingId);

        if (saving is null)
            return;

        saving.CurrentAmount += amount;

        await _savingRepository.UpdateAsync(saving);

        await _savingHistoryRepository.AddAsync(new SavingHistoryEntity
        {
            SavingId = saving.Id,
            Amount = amount,
            Date = DateTime.Now,
            Type = "AddToSaving"
        });
    }

    public async Task<IEnumerable<SavingDto>> GetAllSavingsAsync(string userId)
    {
        var savings = await _savingRepository.GetAllAsync(userId);

        return savings.Select(SavingFactory.ToDto);
    }

    public async Task<SavingDto?> GetSavingByIdAsync(int id)
    {
        var saving = await _savingRepository.GetByIdAsync(id);

        if (saving == null)
            return null;

        return SavingFactory.ToDto(saving);
    }

    public async Task<decimal> GetTotalSavedInPeriodAsync(
        string userId,
        DateTime start,
        DateTime endInclusive)
    {
        var savings = await _savingRepository.GetAllAsync(userId);

        return savings
            .SelectMany(s => s.SavingHistory ?? Enumerable.Empty<SavingHistoryEntity>())
            .Where(h => h.Date >= start && h.Date <= endInclusive)
            .Sum(h => h.Amount);
    }

    public async Task<IEnumerable<SavingHistoryEntity>> GetSavingHistoryAsync(int savingId)
    {
        var history = await _savingHistoryRepository.GetAllAsync();

        return history
            .Where(s => s.SavingId == savingId)
            .OrderByDescending(s => s.Date);
    }

    public async Task<IEnumerable<SavingHistoryEntity>> GetSavingHistoryAsync(string userId)
    {
        return await _savingHistoryRepository.GetAllAsync(userId);
    }

    public async Task<IEnumerable<SavingHistoryEntity>> GetSavingHistoryBySavingIdAsync(int savingId)
    {
        var history = await _savingHistoryRepository.GetAllAsync();

        return history
            .Where(h => h.SavingId == savingId)
            .OrderByDescending(h => h.Date);
    }

    public async Task<SavingDto?> UpdateSavingAsync(
        int id,
        SavingDto updatedSaving,
        string userId)
    {
        var existingSaving = await _savingRepository.GetByIdAsync(id);

        if (existingSaving == null || existingSaving.UserId != userId)
            return null;

        SavingFactory.UpdateEntity(existingSaving, updatedSaving);

        await _savingRepository.UpdateAsync(existingSaving);

        return SavingFactory.ToDto(existingSaving);
    }

    public async Task DeleteSavingAsync(int id, string userId)
    {
        var saving = await _savingRepository.GetByIdAsync(id);

        if (saving == null || saving.UserId != userId)
            return;

        await _savingRepository.DeleteAsync(saving);
    }
}