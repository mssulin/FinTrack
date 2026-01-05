using Business.Interfaces;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class ExpenseService(IExpenseRepository expenseRepository) : IExpenseService
{
    private readonly IExpenseRepository _expenseRepository = expenseRepository;

    public async Task<ExpenseEntity> AddExpenseAsync(ExpenseEntity expense)
    {
        return await _expenseRepository.AddAsync(expense);
    }

    public async Task<IEnumerable<ExpenseEntity>> GetExpensesAsync(string userId)
    {
        var all = await _expenseRepository.GetAllAsync();
        return all.Where(e => e.UserId == userId);
    }
}