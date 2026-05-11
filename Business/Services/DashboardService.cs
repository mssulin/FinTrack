using System.Globalization;
using Business.Dtos;
using Business.Factories;
using Business.Helpers;
using Business.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Business.Services;

public class DashboardService(IConfiguration config, ISavingService savingService, IExpenseService expenseService, IIncomeService incomeService,
    ISubService subService)
    : IDashboardService
{
    private readonly IConfiguration _config = config;
    private readonly IIncomeService _incomeService = incomeService;
    private readonly IExpenseService _expenseService = expenseService;
    private readonly ISubService _subService = subService;
    private readonly ISavingService _savingService = savingService;

    public async Task<DashboardDto> GetDashboardAsync(string userId)
    {
        DateTime start;
        DateTime endInclusive;
        DateTime endOfPeriodForHeader;

        if (_config.GetValue<bool>("DisableMonthlyReset"))
        {
            start = DateTime.MinValue;
            endInclusive = DateTime.MaxValue;
            endOfPeriodForHeader = DateTime.Now;
        }
        else
        {
            var (startOfPeriod, endOfPeriod) = MonthlyPeriodCalculator.GetPeriod(DateTime.Now);

            start = startOfPeriod.Date;
            endInclusive = endOfPeriod.Date.AddDays(1).AddTicks(-1);
            endOfPeriodForHeader = endOfPeriod;
        }
        
        var incomes = (await _incomeService.GetIncomesAsync(userId)).ToList();

        var totalIncome = incomes
            .Where(i => i.Date >= start && i.Date <= endInclusive)
            .Sum(i => i.Amount);
        
        var expenses = (await _expenseService.GetExpensesAsync(userId)).ToList();

        var expenseDtos = expenses
            .Where(e => e.Date >= start && e.Date <= endInclusive)
            .Select(e => new ExpenseDto
            {
                Id = e.Id,
                Category = e.Category,
                Amount = e.Amount
            })
            .ToList();

        var totalExpenses = expenseDtos.Sum(e => e.Amount);

        var today = DateTime.Today;
        var windowEnd = today.AddDays(14);
        
        var subs = await _subService.GetSubsAsync();

        var subDtos = subs
            .Where(s =>
            {
                var inWindow = s.NextPaymentDate.Date >= today &&
                               s.NextPaymentDate.Date <= windowEnd;

                var cycleStart = new DateTime(
                    s.NextPaymentDate.Year,
                    s.NextPaymentDate.Month,
                    1);

                var cycleEnd = cycleStart.AddMonths(1).AddTicks(-1);

                var paidThisUpcomingCycle =
                    s.IsPaid &&
                    s.LastPaymentDate >= cycleStart &&
                    s.LastPaymentDate <= cycleEnd;

                return inWindow && !paidThisUpcomingCycle;
            })
            .OrderBy(s => s.NextPaymentDate)
            .ToList();
        
        var savings = (await _savingService.GetAllSavingsAsync(userId)).ToList();

        var savingHistory = (await _savingService.GetSavingHistoryAsync(userId)).ToList();
        
        var totalSavedThisMonth = savingHistory
            .Where(h => h.Date >= start && h.Date <= endInclusive)
            .Sum(h => h.Amount);
        
        var totalIncomeAllTime = incomes.Sum(i => i.Amount);
        var totalExpensesAllTime = expenses.Sum(e => e.Amount);
        var totalSavedAllTime = savingHistory.Sum(h => h.Amount);

        var runningBalance =
            totalIncomeAllTime -
            totalExpensesAllTime -
            totalSavedAllTime;
        
        var savingPercent = totalIncome > 0
            ? Math.Round((totalSavedThisMonth / totalIncome) * 100, 1)
            : 0;
        
        var culture = new CultureInfo("sv-SE");

        var currentMonth = culture.TextInfo.ToTitleCase(
            endOfPeriodForHeader.ToString("MMMM yyyy", culture));

        return DashboardFactory.Create(
            totalIncome,
            totalExpenses,
            runningBalance,
            expenseDtos,
            subDtos,
            savings,
            currentMonth,
            savingPercent
        );
    }
}