using SalaryCalculator.App.Shared.Utils.Domain;

namespace SalaryCalculator.App.Domain.Services;

public class DependencySalaryCalculator
{
    public SalaryEstimation Calculate(
        decimal salarySigned,
        decimal bonus,
        DateOnly initialDate,
        DateOnly finalDate,
        bool accumulatedBenefits
    )
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        var benefitsPerMonth = GetLawBenefits(
            new DateOnly(today.Year, today.Month, 1),
            new DateOnly(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month)),
            salarySigned);

        var benefitsInTime = GetLawBenefits(initialDate, finalDate, salarySigned);

        var liquidityAmountPerMonth =
            salarySigned - GetContribution(Salary.Dependency[SalaryKeys.WorkerContributionPercent], salarySigned) +
            bonus;
        liquidityAmountPerMonth += (!accumulatedBenefits ? benefitsPerMonth : 0m);

        var employerAmountPerMonth =
            salarySigned + GetContribution(Salary.Dependency[SalaryKeys.EmployerContributionPercent], salarySigned) +
            bonus + benefitsPerMonth;

        var liquidityAmountInTime = ProportionalCalculator.GetTotalAmountInTime(
            initialDate,
            finalDate,
            liquidityAmountPerMonth);

        var employerAmountInTime = ProportionalCalculator.GetTotalAmountInTime(
            initialDate,
            finalDate,
            employerAmountPerMonth);

        return SalaryEstimation.CreateForDependencyContract(
            liquidityAmountPerMonth,
            employerAmountPerMonth,
            liquidityAmountInTime,
            employerAmountInTime,
            [
                new { month = accumulatedBenefits ? Math.Round(benefitsPerMonth, 2) : 0m },
                new { periodChoose = accumulatedBenefits ? Math.Round(benefitsInTime, 2) : 0m }
            ],
            salarySigned,
            initialDate,
            finalDate
        );
    }

    private decimal GetLawBenefits(DateOnly initialDate, DateOnly finalDate, decimal salarySigned)
    {
        var monthsWorked = ProportionalCalculator.GetMonthsProportionYears(initialDate, finalDate)
                           + ProportionalCalculator.GetMonths(initialDate, finalDate)
                           + ProportionalCalculator.GetMonthsProportionDays(initialDate, finalDate);

        return CalculateChristmasBonus(monthsWorked, salarySigned)
               + CalculateScholarBonus(monthsWorked)
               + CalculateVacationBonus(monthsWorked, salarySigned);
    }

    private static decimal GetContribution(decimal contributionPercent, decimal salarySigned)
    {
        return salarySigned * (contributionPercent / 100);
    }

    private decimal CalculateScholarBonus(decimal monthsWorked)
    {
        return Salary.Dependency[SalaryKeys.Sbu] * monthsWorked / 12;
    }

    private decimal CalculateChristmasBonus(decimal monthsWorked, decimal salarySigned)
    {
        return (salarySigned * monthsWorked) / 12;
    }

    private decimal CalculateVacationBonus(decimal monthsWorked, decimal salarySigned)
    {
        const int DaysInMonth = 30;
        const int MonthsInYear = 12;
        const int VacationDays = 15;
        var vacationBonusPerYear = VacationDays * (salarySigned / DaysInMonth);
        var vacationBonusPerMonthsWorked = (MonthsInYear * monthsWorked) / (vacationBonusPerYear);
        return vacationBonusPerMonthsWorked;
    }
}