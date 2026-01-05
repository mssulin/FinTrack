using Data.Entities;

namespace Data.Interfaces;

public interface IIncomeRepository : IBaseRepository<IncomeEntity>
{
    Task<IEnumerable<IncomeEntity>> GetAllAsync(string userId);
}