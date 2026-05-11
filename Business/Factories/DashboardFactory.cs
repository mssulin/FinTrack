using Business.Dtos;

namespace Business.Factories;

public static class DashboardFactory
{
    public static DashboardDto Create(
        decimal totalIncome,
        decimal totalExpenses,
        decimal availableMoney,
        List<ExpenseDto> expenses,
        List<SubscriptionDto> subscriptions,
        List<SavingDto> savings,
        string currentMonth,
        decimal savingPercent)
    {
        return new DashboardDto
        {
            TotalIncome = totalIncome,
            TotalExpenses = totalExpenses,
            AvailableMoney = availableMoney,
            Expenses = expenses,
            Subscriptions = subscriptions,
            Savings = savings,
            CurrentMonth = currentMonth,
            SavingPercent = savingPercent,
            ExpenseCategories = new List<string>
            {
                "Boende",
                "Mat",
                "Transport",
                "Shopping",
                "Nöje",
                "Husdjur"
            },
            IncomeSources = new List<string>
            {
                "Lön",
                "CSN",
                "Dricks",
                "Försäljning"
            }
        };
    }
}