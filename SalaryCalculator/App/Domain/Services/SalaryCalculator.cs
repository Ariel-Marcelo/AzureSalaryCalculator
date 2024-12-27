namespace SalaryCalculator.App.Domain.Services;

public abstract class SalaryCalculator
{
    protected decimal GetTotalAmountInTime(DateOnly initialDate, DateOnly finalDate, decimal amountPerMonth)
    {
        var yearsAmount = (finalDate.Year - initialDate.Year) * 12;
        var monthAmount = finalDate.Month - initialDate.Month;
        var daysFirstMonth = monthAmount > 0
            ? DateTime.DaysInMonth(initialDate.Year, initialDate.Month) - initialDate.Day
            : 0;
        
        var daysLastMonth = finalDate.Day;
        decimal dayAmount = (daysLastMonth + daysFirstMonth) /
                        (decimal)DateTime.DaysInMonth(finalDate.Year, finalDate.Month);

        return (yearsAmount + monthAmount + dayAmount) * amountPerMonth;
    }
    
    
}