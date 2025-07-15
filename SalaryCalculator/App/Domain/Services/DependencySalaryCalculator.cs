using SalaryCalculator.App.Shared.Utils.Domain;

namespace SalaryCalculator.App.Domain.Services;

public class DependencySalaryCalculator
{
    public SalaryEstimation Calculate(
        decimal salarySigned,
        decimal bonus,
        DateOnly initialDate,
        DateOnly finalDate,
        bool accumulatedBenefits,
        decimal utilities,
        bool hasReserveFunds
    )
    {
        var benefitsPerMonth = GetLawBenefits(salarySigned, 1, hasReserveFunds);
        var monthsInTime = ProportionalCalculator.GetMonthsProportionYears(initialDate, finalDate)
                           + ProportionalCalculator.GetMonths(initialDate, finalDate)
                           + ProportionalCalculator.GetMonthsProportionDays(initialDate, finalDate);
        var benefitsInTime = GetLawBenefits(salarySigned, monthsInTime, hasReserveFunds);

        var utilitiesPerMonth = CalculateUtilities(1, utilities);
        var utilitiesInTime = CalculateUtilities(monthsInTime, utilities);


        var liquidityAmountPerMonth =
            salarySigned - GetContribution(Salary.Dependency[SalaryKeys.WorkerContributionPercent], salarySigned) +
            bonus;
        liquidityAmountPerMonth += !accumulatedBenefits ? benefitsPerMonth : 0m;

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
            finalDate,
            utilitiesPerMonth,
            utilitiesInTime
        );
    }

    private decimal GetLawBenefits(decimal salarySigned, decimal monthsWorked, bool hasReservFounds)
    {
        return CalculateChristmasBonus(monthsWorked, salarySigned)
               + CalculateScholarBonus(monthsWorked)
               + CalculateVacationBonus(monthsWorked, salarySigned)
               + (hasReservFounds ? CalculateReservFounds(salarySigned) : 0);
    }

    private decimal CalculateReservFounds(decimal salary)
    {
        return salary * 8.33m / 100;
    }

    private decimal CalculateUtilities(decimal monthsWorked, decimal utilities)
    {
        return utilities * monthsWorked / 12;
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
        return salarySigned * monthsWorked / 12;
    }

    private decimal CalculateVacationBonus(decimal monthsWorked, decimal salarySigned)
    {
        const int DaysInMonth = 30;
        const int MonthsInYear = 12;
        const int VacationDays = 15;
        var vacationBonusPerYear = VacationDays * (salarySigned / DaysInMonth);
        var vacationBonusPerMonthsWorked = MonthsInYear * monthsWorked / vacationBonusPerYear;
        return vacationBonusPerMonthsWorked;
    }
}