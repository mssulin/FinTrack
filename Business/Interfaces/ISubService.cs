using Business.Dtos;

namespace Business.Interfaces;

public interface ISubService
{
    Task<SubscriptionDto> AddSubAsync(SubscriptionDto subscription, string userId);

    Task<IEnumerable<SubscriptionDto>> GetSubsAsync();

    Task<SubscriptionDto?> GetSubByIdAsync(int id);

    Task<SubscriptionDto?> UpdateSubAsync(int id, SubscriptionDto updatedSub, string userId);

    Task DeleteSubAsync(int id, string userId);

    Task MarkPaidAsync(int id);
}