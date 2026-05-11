using Business.Dtos;
using Data.Entities;
using WebApp.ViewModels;

namespace WebApp.Mappers;

public static class SavingMapper
{
    public static SavingViewModel ToViewModel(SavingDto dto)
    {
        return new SavingViewModel
        {
            Id = dto.Id,
            Title = dto.Title,
            CurrentAmount = dto.CurrentAmount,
            TargetAmount = dto.TargetAmount
        };
    }

    public static SavingDto ToDto(SavingViewModel model)
    {
        return new SavingDto
        {
            Id = model.Id,
            Title = model.Title,
            CurrentAmount = model.CurrentAmount,
            TargetAmount = model.TargetAmount
        };
    }
}