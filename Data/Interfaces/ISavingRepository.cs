using Data.Entities;

namespace Data.Interfaces;

public interface ISavingRepository : IBaseRepository<SavingEntity>
{
    Task<IEnumerable<SavingEntity>> GetAllAsync(string userId);
}