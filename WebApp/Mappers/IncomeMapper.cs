using Data.Entities;
using WebApp.ViewModels;

namespace WebApp.Mappers;

public static class IncomeMapper
{
    public static IncomeEntity ToEntity(DashboardViewModel model, string userId)
    {
        return new IncomeEntity
        {
            Source = model.NewIncome.Source,
            Amount = model.NewIncome.Amount,
            UserId = userId
        };
    }
}