using Business.Dtos;
using WebApp.ViewModels;

namespace WebApp.Factories;

public static class SubscriptionViewModelFactory
{
    public static SubscriptionDto ToDto(SubscriptionViewModel model)
    {
        return new SubscriptionDto
        {
            Id = model.Id,
            Name = model.Name,
            Amount = model.Amount,
            LastPaymentDate = model.LastPaymentDate,
            NextPaymentDate = model.NextPaymentDate,
            Category = model.Category,
            Frequency = model.Frequency,
            ImageUrl = model.ImageUrl,
            IsPaid = model.IsPaid
        };
    }

    public static SubscriptionViewModel ToViewModel(SubscriptionDto dto)
    {
        return new SubscriptionViewModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Amount = dto.Amount,
            LastPaymentDate = dto.LastPaymentDate,
            NextPaymentDate = dto.NextPaymentDate,
            Category = dto.Category,
            Frequency = dto.Frequency,
            ImageUrl = dto.ImageUrl,
            IsPaid = dto.IsPaid
        };
    }
}