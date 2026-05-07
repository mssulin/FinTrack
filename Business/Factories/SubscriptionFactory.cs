using Business.Dtos;
using Data.Entities;

namespace Business.Factories;

public static class SubscriptionFactory
{
    public static SubscriptionDto ToDto(SubscriptionEntity entity)
    {
        return new SubscriptionDto
        {
            Id = entity.Id,
            Name = entity.Name,
            ImageUrl = entity.ImageUrl,
            Amount = entity.Amount,
            Category = entity.Category,
            Frequency = entity.Frequency,
            LastPaymentDate = entity.LastPaymentDate,
            NextPaymentDate = entity.NextPaymentDate,
            IsPaid = entity.IsPaid
        };
    }

    public static SubscriptionEntity ToEntity(SubscriptionDto dto, string userId)
    {
        return new SubscriptionEntity
        {
            Id = dto.Id,
            Name = dto.Name,
            ImageUrl = dto.ImageUrl,
            Amount = dto.Amount,
            Category = dto.Category,
            Frequency = dto.Frequency,
            LastPaymentDate = dto.LastPaymentDate,
            NextPaymentDate = dto.NextPaymentDate,
            IsPaid = dto.IsPaid,
            UserId = userId
        };
    }

    public static void UpdateEntity(SubscriptionEntity entity, SubscriptionDto dto)
    {
        entity.Name = dto.Name;
        entity.ImageUrl = dto.ImageUrl;
        entity.Amount = dto.Amount;
        entity.Category = dto.Category;
        entity.Frequency = dto.Frequency;
        entity.LastPaymentDate = dto.LastPaymentDate;
        entity.NextPaymentDate = dto.NextPaymentDate;
        entity.IsPaid = dto.IsPaid;
    }
}