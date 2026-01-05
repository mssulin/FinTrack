using Data.Entities;

namespace Data.Interfaces;

public interface ISavingHistoryRepository : IBaseRepository<SavingHistoryEntity>
{
    Task<IEnumerable<SavingHistoryEntity>> GetAllAsync(string userId);
}