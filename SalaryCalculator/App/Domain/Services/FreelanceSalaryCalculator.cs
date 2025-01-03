using SalaryCalculator.App.Shared.Utils.Domain;

namespace SalaryCalculator.App.Domain.Services;

public class FreelanceSalaryCalculator
{
    public SalaryEstimation Calculate(
        decimal salarySigned,
        bool taxIncluded,
        decimal contributionSalary,
        DateOnly initialDate,
        DateOnly finalDate
    )
    {
        var taxReverseWorkerPercent = taxIncluded
            ? 100m / (100m + Salary.Freelance[SalaryKeys.TaxWorkerPercent])
            : 1;

        var liquidityPerMonthAmount = salarySigned * taxReverseWorkerPercent;

        var employerAmountPerMonth = taxIncluded
            ? salarySigned
            : liquidityPerMonthAmount * ((100 + Salary.Freelance[SalaryKeys.TaxWorkerPercent]) / 100m);

        if (contributionSalary > 0)
        {
            var contributionAmount =
                contributionSalary * (Salary.Freelance[SalaryKeys.WorkerContributionPercent] / 100m);
            liquidityPerMonthAmount -= contributionAmount;
        }

        var liquidityAmountInTime = ProportionalCalculator.GetTotalAmountInTime(
            initialDate,
            finalDate,
            liquidityPerMonthAmount);

        var employerAmountInTime = ProportionalCalculator.GetTotalAmountInTime(
            initialDate,
            finalDate,
            employerAmountPerMonth);

        return SalaryEstimation.CreateForFreelanceContract(
            employerAmountPerMonth,
            liquidityPerMonthAmount,
            employerAmountInTime,
            liquidityAmountInTime,
            [
                new
                {
                    services = taxIncluded
                        ? -(salarySigned - liquidityPerMonthAmount)
                        : -(salarySigned * Salary.Freelance[SalaryKeys.TaxWorkerPercent] / 100)
                }
            ],
            contributionSalary,
            initialDate,
            finalDate
        );
    }
}