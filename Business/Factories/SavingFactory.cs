using Business.Dtos;
using Data.Entities;

namespace Business.Factories;

public static class SavingFactory
{
    public static SavingDto ToDto(SavingEntity entity)
    {
        return new SavingDto
        {
            Id = entity.Id,
            Title = entity.Title,
            CurrentAmount = entity.CurrentAmount,
            TargetAmount = entity.TargetAmount
        };
    }

    public static SavingEntity ToEntity(SavingDto dto, string userId)
    {
        return new SavingEntity
        {
            Id = dto.Id,
            Title = dto.Title,
            CurrentAmount = dto.CurrentAmount,
            TargetAmount = dto.TargetAmount,
            UserId = userId
        };
    }

    public static void UpdateEntity(
        SavingEntity entity,
        SavingDto dto)
    {
        entity.Title = dto.Title;
        entity.CurrentAmount = dto.CurrentAmount;
        entity.TargetAmount = dto.TargetAmount;
    }
}