namespace SalaryCalculator.App.Shared.Utils.Domain;

public class ProportionalCalculator
{
    public static decimal GetTotalAmountInTime(
        DateOnly initialDate,
        DateOnly finalDate,
        decimal amountPerMonth
    )
    {
        var yearsAmount = GetMonthsProportionYears(initialDate, finalDate);
        
        var monthAmount = GetMonths(initialDate, finalDate);

        var dayAmount = GetMonthsProportionDays(initialDate, finalDate);

        return (yearsAmount + monthAmount + dayAmount) * amountPerMonth;
    }

    public static int GetMonths(DateOnly initialDate, DateOnly finalDate)
    {
        return finalDate.Month - initialDate.Month;
    }

    public static int GetMonthsProportionYears(DateOnly initialDate, DateOnly finalDate)
    {
        var yearsAmount = (finalDate.Year - initialDate.Year) * 12;
        return yearsAmount;
    }

    public static decimal GetMonthsProportionDays(
        DateOnly initialDate, 
        DateOnly finalDate)
    {
        var daysWorkedFirstMonth =
            ((decimal)DateTime.DaysInMonth(initialDate.Year, initialDate.Month) - initialDate.Day + 1) /
            DateTime.DaysInMonth(initialDate.Year, initialDate.Month);

        var daysWorkedLastMonth = finalDate.Day / DateTime.DaysInMonth(finalDate.Year, finalDate.Month);

        var monthProportionDays = (1 + (daysWorkedLastMonth - daysWorkedFirstMonth));
        return monthProportionDays;
    }
}