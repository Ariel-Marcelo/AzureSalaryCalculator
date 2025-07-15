using Microsoft.Net.Http.Headers;

namespace SalaryCalculator.App.Domain;

public class SalaryEstimation
{
    public decimal EnterpriseMonthAmount { get; set; }

    public decimal LiquidityMonthAmount { get; set; }

    public decimal IeesSalary { get; set; }

    public DateOnly StartDate { get; set; }
    
    public DateOnly FinalDate { get; set; }
    
    public decimal EnterpriseAmountInTime { get; set; }

    public decimal LiquidityAmountInTime { get; set; }
    
    public int DaysWorked { get; set; }

    public int MonthsWorked { get; set; }

    public int YearsWorked { get; set; }

    public decimal UtilitiesPerMonth { get; set; }

    public decimal UtilitiesInTime { get; set; }

    public IEnumerable<object> Details { get; set; }

    private SalaryEstimation(
        decimal enterpriseMonthAmount,
        decimal liquidityMonthAmount,
        IEnumerable<object> benefitsPerMonth,
        decimal enterpriseAmountInTime,
        decimal liquidityAmountInTime,
        decimal ieesSalary,
        DateOnly startDate,
        DateOnly finalDate,
        int daysWorked,
        int monthsWorked,
        int yearsWorked,
        decimal utilitiesPerMonth,
        decimal utilitiesInTime
    )
    {
        EnterpriseMonthAmount = enterpriseMonthAmount;
        LiquidityMonthAmount = liquidityMonthAmount;
        Details = benefitsPerMonth;
        EnterpriseAmountInTime = enterpriseAmountInTime;
        LiquidityAmountInTime = liquidityAmountInTime;
        StartDate = startDate;
        FinalDate = finalDate;
        IeesSalary = ieesSalary;
        DaysWorked = daysWorked;
        MonthsWorked = monthsWorked;
        YearsWorked = yearsWorked;
        UtilitiesPerMonth = utilitiesPerMonth;
        UtilitiesInTime = utilitiesInTime;
    }

    public static SalaryEstimation CreateForFreelanceContract(
        decimal totalAmount,
        decimal liquidityAmount,
        decimal totalAmountInTime,
        decimal liquidityAmountInTime,
        IEnumerable<object> details,
        decimal ieesSalary,
        DateOnly startDate,
        DateOnly finalDate
    )
    {
        return new SalaryEstimation(
            Math.Round(totalAmount,2),
            Math.Round(liquidityAmount, 2),
            details,
            Math.Round(totalAmountInTime, 2),
            Math.Round(liquidityAmountInTime, 2),
            Math.Round(ieesSalary, 2),
            startDate,
            finalDate,
            finalDate.Day - startDate.Day + 1,
            finalDate.Month - startDate.Month,
            finalDate.Year - startDate.Year,
            0,
            0
        );
    }

    public static SalaryEstimation CreateForDependencyContract(
        decimal liquidityAmount,
        decimal totalAmount,
        decimal liquidityAmountInTime,
        decimal totalAmountInTime,
        IEnumerable<object> benefitsPerMonth,
        decimal ieesSalary,
        DateOnly startDate,
        DateOnly finalDate,
        decimal utilitiesPerMonth,
        decimal utilitiesInTime
    )
    {
        return new SalaryEstimation(
            Math.Round(totalAmount, 2),
            Math.Round(liquidityAmount, 2),
            benefitsPerMonth,
            Math.Round(totalAmountInTime, 2),
            Math.Round(liquidityAmountInTime, 2),
            Math.Round(ieesSalary, 2),
            startDate,
            finalDate,
            finalDate.Day - startDate.Day + 1,
            finalDate.Month - startDate.Month,
            finalDate.Year - startDate.Year,
            Math.Round(utilitiesPerMonth, 2),
            Math.Round(utilitiesInTime, 2)
        );
    }
}