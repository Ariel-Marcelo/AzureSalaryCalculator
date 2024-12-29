
namespace SalaryCalculator.App.Domain.Services;

public class DependencySalaryCalculator: SalaryCalculator
{
    private const decimal TAX_WORKER = 9.45m;
    private const decimal TAX_EMPLOYER = 11.15m;
    private const decimal SBU = 470;

    public SalaryEstimation Calculate(
        decimal salarySigned,
        decimal bonus,
        DateOnly initialDate,
        DateOnly finalDate,
        bool accumulatedBenefits
    )
    {
        var taxAmount = salarySigned * (TAX_WORKER / 100);
        var liquidityAmountPerMonth = salarySigned - taxAmount + bonus;

        var taxEmployer = salarySigned * (TAX_EMPLOYER / 100);
        var totalAmountPerMonth = salarySigned + taxEmployer + bonus;

        var liquidityAmountInTime = GetTotalAmountInTime(initialDate, finalDate, liquidityAmountPerMonth);
        var totalAmountInTime = GetTotalAmountInTime(initialDate, finalDate, totalAmountPerMonth);

        var benefitsPerMonth = CalculateBenefits(
            DateOnly.FromDateTime(DateTime.Now),
            DateOnly.FromDateTime(DateTime.Now).AddMonths(1),
            salarySigned);
        var benefitsInTime = CalculateBenefits(initialDate, finalDate, salarySigned);
        
        return SalaryEstimation.CreateForDependencyContract(
            liquidityAmountPerMonth + (!accumulatedBenefits ? benefitsPerMonth : 0m),
            totalAmountPerMonth + benefitsPerMonth,
            liquidityAmountInTime + (!accumulatedBenefits ? benefitsInTime : 0m),
            totalAmountInTime + benefitsInTime,
            [
                new { month = accumulatedBenefits ? Math.Round(benefitsPerMonth, 2) : 0m },
                new { periodChoose = accumulatedBenefits ? Math.Round(benefitsInTime, 2) : 0m }
            ],
            salarySigned,
            initialDate,
            finalDate
        );
    }

    private decimal CalculateBenefits(DateOnly initialDate, DateOnly finalDate, decimal salarySigned)
    {
        var dayInStartMonth = DateTime.DaysInMonth(initialDate.Year, initialDate.Month);
        var dayInEndMonth = DateTime.DaysInMonth(finalDate.Year, finalDate.Month);
        var monthProportionFirstMonth = (dayInStartMonth - initialDate.Day) / dayInStartMonth;
        var monthProportionLastMonth = finalDate.Day / dayInEndMonth;

        var monthsWorked = (finalDate.Year - initialDate.Year) * 12
                           + (finalDate.Month - initialDate.Month)
                           + monthProportionFirstMonth
                           + monthProportionLastMonth;
        
        return CalculateChristmasBonus(monthsWorked, salarySigned) 
               + CalculateScholarBonus(monthsWorked)
               + CalculateVacationBonus(monthsWorked, salarySigned);
    }

    private decimal CalculateScholarBonus(decimal monthsWorked)
    {
        return SBU * monthsWorked / 12;
    }

    private decimal CalculateChristmasBonus(decimal monthsWorked, decimal salarySigned)
    {
        return (salarySigned * monthsWorked) / 12;
    }

    private decimal CalculateVacationBonus(decimal monthsWorked, decimal salarySigned)
    {
        var x = (0.5m/12m) * (monthsWorked);
        return salarySigned * x;
    }
}