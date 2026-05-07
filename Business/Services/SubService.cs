using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Data.Interfaces;

namespace Business.Services;

public class SubService(ISubscriptionRepository subRepository) : ISubService
{
    private readonly ISubscriptionRepository _subRepository = subRepository;

    public async Task<SubscriptionDto> AddSubAsync(SubscriptionDto subscription, string userId)
    {
        var entity = SubscriptionFactory.ToEntity(subscription, userId);

        var result = await _subRepository.AddAsync(entity);

        return SubscriptionFactory.ToDto(result);
    }

    public async Task<IEnumerable<SubscriptionDto>> GetSubsAsync()
    {
        var subscriptions = await _subRepository.GetAllAsync();

        return subscriptions.Select(SubscriptionFactory.ToDto);
    }

    public async Task<SubscriptionDto?> GetSubByIdAsync(int id)
    {
        var subscription = await _subRepository.GetByIdAsync(id);

        if (subscription == null)
            return null;

        return SubscriptionFactory.ToDto(subscription);
    }

    public async Task<SubscriptionDto?> UpdateSubAsync(int id, SubscriptionDto updatedSub, string userId)
    {
        var existingSub = await _subRepository.GetByIdAsync(id);

        if (existingSub == null || existingSub.UserId != userId)
            return null;

        SubscriptionFactory.UpdateEntity(existingSub, updatedSub);

        await _subRepository.UpdateAsync(existingSub);

        return SubscriptionFactory.ToDto(existingSub);
    }

    public async Task DeleteSubAsync(int id, string userId)
    {
        var subscription = await _subRepository.GetByIdAsync(id);

        if (subscription == null || subscription.UserId != userId)
            return;

        await _subRepository.DeleteAsync(subscription);
    }

    public async Task MarkPaidAsync(int id)
    {
        var sub = await _subRepository.GetByIdAsync(id);

        if (sub is null)
            return;

        sub.LastPaymentDate = DateTime.Now;

        sub.NextPaymentDate = sub.Frequency switch
        {
            "Månad" => sub.NextPaymentDate.AddMonths(1),
            "Vecka" => sub.NextPaymentDate.AddDays(7),
            _ => sub.NextPaymentDate
        };

        sub.IsPaid = false;

        await _subRepository.UpdateAsync(sub);
    }
}