using Data.Entities;

namespace Business.Interfaces;

public interface IExpenseService
{
    Task<ExpenseEntity> AddExpenseAsync(ExpenseEntity expense);
    Task<IEnumerable<ExpenseEntity>> GetExpensesAsync(string userId);
}