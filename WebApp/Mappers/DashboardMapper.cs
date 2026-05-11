using WebApp.ViewModels;

namespace WebApp.Mappers;

public static class DashboardMapper
{
    public static DashboardViewModel ToViewModel(Business.Dtos.DashboardDto dto)
    {
        return new DashboardViewModel
        {
            TotalIncome = dto.TotalIncome,
            TotalExpenses = dto.TotalExpenses,
            AvailableMoney = dto.AvailableMoney,

            Expenses = dto.Expenses
                .Select(ExpenseMapper.ToViewModel)
                .ToList(),

            Subscriptions = dto.Subscriptions
                .Select(SubscriptionMapper.ToViewModel)
                .ToList(),

            Savings = dto.Savings
                .OrderByDescending(s => s.TargetAmount > 0
                    ? s.CurrentAmount / s.TargetAmount
                    : 0)
                .Select(SavingMapper.ToViewModel)
                .ToList(),

            CurrentMonth = dto.CurrentMonth,
            SavingPercent = dto.SavingPercent,
            ExpenseCategories = dto.ExpenseCategories,
            IncomeSources = dto.IncomeSources
        };
    }
}