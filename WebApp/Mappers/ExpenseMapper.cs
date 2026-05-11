using Data.Entities;
using WebApp.ViewModels;

namespace WebApp.Mappers;

public static class ExpenseMapper
{
    public static ExpenseEntity ToEntity(ExpenseViewModel model, string userId)
    {
        return new ExpenseEntity
        {
            Date = DateTime.Now,
            Amount = model.Amount,
            Category = model.Category,
            UserId = userId
        };
    }

    public static ExpenseViewModel ToViewModel(Business.Dtos.ExpenseDto dto)
    {
        return new ExpenseViewModel
        {
            Id = dto.Id,
            Category = dto.Category,
            Amount = dto.Amount
        };
    }
}