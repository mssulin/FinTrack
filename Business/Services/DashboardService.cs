using System.Globalization;
using Business.Dtos;
using Business.Helpers;
using Business.Interfaces;
using Data.Entities;
using Microsoft.Extensions.Configuration; 

namespace Business.Services;

public class DashboardService(
    IConfiguration config,
    ISavingService savingService,
    IExpenseService expenseService,
    IIncomeService incomeService,
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
        // Start och slutdatum för period (25:e varje månad)
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

        // Inkomster
        var incomes = await _incomeService.GetIncomesAsync(userId);
        var totalIncome = incomes
            .Where(i => i.Date >= start && i.Date <= endInclusive)
            .Sum(i => i.Amount);

        // Utgifter
        var expenses = await _expenseService.GetExpensesAsync(userId);
        var expenseDtos = expenses
            .Where(e => e.Date >= start && e.Date <= endInclusive)
            .Select(e => new ExpenseDto
            {
                Id = e.Id,
                Category = e.Category,
                Amount = e.Amount,
            })
            .ToList();

        var totalExpenses = expenseDtos.Sum(e => e.Amount);

        var today = DateTime.Today;
        var windowEnd = today.AddDays(14);

        // Prenumerationer de 14 kommande dagarna
        var subs = await _subService.GetSubsAsync();

        var subDtos = subs
            .Where(s =>
            {
                // Beräkning för om nästa betalningsdatum är inom 14 dagar
                var inWindow = s.NextPaymentDate.Date >= today &&
                               s.NextPaymentDate.Date <= windowEnd;

                // Kontrollerar om det är betalt för denna period
                var cycleStart = new DateTime(s.NextPaymentDate.Year, s.NextPaymentDate.Month, 1);
                var cycleEnd = cycleStart.AddMonths(1).AddTicks(-1);

                var paidThisUpcomingCycle =
                    s.IsPaid &&
                    s.LastPaymentDate >= cycleStart &&
                    s.LastPaymentDate <= cycleEnd;

                // Visas bara om den inte är betald
                return inWindow && !paidThisUpcomingCycle;
            })
            .Select(s => new SubscriptionDto
            {
                Id = s.Id,
                Name = s.Name,
                ImageUrl = s.ImageUrl,
                Amount = s.Amount,
                Category = s.Category,
                Frequency = s.Frequency,
                LastPaymentDate = s.LastPaymentDate,
                NextPaymentDate = s.NextPaymentDate,
                IsPaid = s.IsPaid,
            })
            .OrderBy(s => s.NextPaymentDate)
            .ToList();

        // Sparmål
        var savings = await _savingService.GetAllSavingsAsync(userId);

        var savingDtos = savings.Select(s => new SavingDto
        {
            Id = s.Id,
            Title = s.Title,
            CurrentAmount = s.CurrentAmount,   
            TargetAmount = s.TargetAmount,
        }).ToList();

        // Totalt sparat under nuvarande månad
        var totalSavedThisMonth = savings
            .SelectMany(s => s.SavingHistory ?? Enumerable.Empty<SavingHistoryEntity>())
            .Where(h => h.Date >= start && h.Date <= endInclusive)
            .Sum(h => h.Amount);

        // Total balans över tid (inkomster - utgifter - sparande)
        var totalIncomeAllTime = incomes.Sum(i => i.Amount);
        var totalExpensesAllTime = expenses.Sum(e => e.Amount);
        var totalSavedAllTime = savings
            .SelectMany(s => s.SavingHistory ?? Enumerable.Empty<SavingHistoryEntity>())
            .Sum(h => h.Amount);

        var runningBalance = totalIncomeAllTime - totalExpensesAllTime - totalSavedAllTime;

        // Sparprocent för månaden (sparat / inkomst)
        var savingPercent = totalIncome > 0
            ? Math.Round((totalSavedThisMonth / totalIncome) * 100, 1)
            : 0;

        // Formattering för nuvarande månad 
        var culture = new CultureInfo("sv-SE");
        var currentMonth = culture.TextInfo.ToTitleCase(endOfPeriodForHeader.ToString("MMMM yyyy", culture));

        return new DashboardDto
        {
            TotalIncome = totalIncome,
            TotalExpenses = totalExpenses,
            
            AvailableMoney = runningBalance,

            Expenses = expenseDtos,
            Subscriptions = subDtos,
            Savings = savingDtos,
            CurrentMonth = currentMonth,
            SavingPercent = savingPercent,
            ExpenseCategories = new List<string> { "Boende", "Mat", "Transport", "Shopping", "Nöje", "Husdjur" },
            IncomeSources = new List<string> { "Lön", "CSN", "Dricks", "Försäljning" }
        };
    }
}