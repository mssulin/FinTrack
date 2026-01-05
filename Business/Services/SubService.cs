using Business.Interfaces;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class SubService(ISubscriptionRepository subRepository) : ISubService
{
    private readonly ISubscriptionRepository _subRepository = subRepository;

    public async Task<SubscriptionEntity> AddSubAsync(SubscriptionEntity subscription)
    {
        return await _subRepository.AddAsync(subscription);
    }

    public async Task<IEnumerable<SubscriptionEntity>> GetSubsAsync()
    {
        return await _subRepository.GetAllAsync();
    }

    public async Task<SubscriptionEntity?> GetSubByIdAsync(int id)
    {
        return await _subRepository.GetByIdAsync(id);
    }

    public async Task UpdateSubAsync(SubscriptionEntity subscription)
    {
        await _subRepository.UpdateAsync(subscription);
    }

    public async Task DeleteSubAsync(SubscriptionEntity subscription)
    {
        await _subRepository.DeleteAsync(subscription);
    }

    // Beräknar nästa betalningsdatum när en prenumeration är markerad som betald
    public async Task MarkPaidAsync(int id)
    {
        var sub = await _subRepository.GetByIdAsync(id);
        if (sub is null) return;

        // Sätter senaste betalningsdatum till nuvarande tid
        sub.LastPaymentDate = DateTime.Now;

        // Uppdatera nästa betalningsdatum beroende på hur ofta den ska betalas
        sub.NextPaymentDate = sub.Frequency switch
        {
            "Månad" => sub.NextPaymentDate.AddMonths(1),
            "Vecka" => sub.NextPaymentDate.AddDays(7),
            _ => sub.NextPaymentDate
        };

        // Reset för prenumerationer när den är betald
        sub.IsPaid = false;

        await _subRepository.UpdateAsync(sub);
    }
}