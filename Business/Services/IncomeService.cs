using Business.Interfaces;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class IncomeService(IIncomeRepository incomeRepository) : IIncomeService
{
    private readonly IIncomeRepository _incomeRepository = incomeRepository;

    public async Task<IncomeEntity> AddIncomeAsync(IncomeEntity income)
    {
        return await _incomeRepository.AddAsync(income);
    }

    public async Task<IEnumerable<IncomeEntity>> GetIncomesAsync(string userId)
    {
        return await _incomeRepository.GetAllAsync(userId);
    }

    public async Task<IncomeEntity?> GetIncomeByIdAsync(int id)
    {
        return await _incomeRepository.GetByIdAsync(id);
    }

    public async Task UpdateIncomeAsync(IncomeEntity income)
    {
        await _incomeRepository.UpdateAsync(income);
    }

    public async Task DeleteIncomeAsync(IncomeEntity income)
    {
        await _incomeRepository.DeleteAsync(income);
    }
}