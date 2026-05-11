
namespace Business.Helpers;

public static class MonthlyPeriodCalculator
{
    private static DateTime AdjustPayDay(DateTime date)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday)
            return date.AddDays(-1);
        
        if (date.DayOfWeek == DayOfWeek.Sunday)
            return date.AddDays(-2);
        
        return date;
    }

    public static (DateTime Start, DateTime End) GetPeriod(DateTime now)
    {
        var thisMonthPayDay = AdjustPayDay(new DateTime(now.Year, now.Month, 25));
        var nextMonthPayDay = AdjustPayDay(new DateTime(now.Year, now.Month, 25).AddMonths(1));

        DateTime start;
        DateTime end;

        if (now >= thisMonthPayDay)
        {
            start = thisMonthPayDay;
            end = nextMonthPayDay.AddDays(-1);
        }
        else
        {
            var lastMonthPayDay = AdjustPayDay(new DateTime(now.Year, now.Month, 25).AddMonths(-1));
            start = lastMonthPayDay;
            end = thisMonthPayDay.AddDays(-1);
        }
        
        return (start, end);
    }
}