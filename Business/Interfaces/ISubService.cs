using Data.Entities;

namespace Business.Interfaces;

public interface ISubService
{
    Task<SubscriptionEntity> AddSubAsync(SubscriptionEntity subscription);
    
    Task<IEnumerable<SubscriptionEntity>> GetSubsAsync();
    
    Task<SubscriptionEntity?> GetSubByIdAsync(int id);
    
    Task UpdateSubAsync(SubscriptionEntity subscription);
    
    Task DeleteSubAsync(SubscriptionEntity subscription);

    Task MarkPaidAsync(int id);
}