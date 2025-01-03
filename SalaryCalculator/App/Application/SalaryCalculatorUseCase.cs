using SalaryCalculator.App.Domain;
using SalaryCalculator.App.Domain.Services;

namespace SalaryCalculator.App.Application;

public class SalaryCalculatorUseCase
{
    public SalaryEstimation CalculateSalaryEstimation(SalaryCalculatorRequest request)
    {
        if (request.IsDependencyContract == true)
        {
            var service = new DependencySalaryCalculator();
            return service.Calculate(
                request.SalarySigned ?? 0,
                request.SalaryBonus,
                request.InitDate,
                request.EndDate,
                request.AccumulatedBenefits
            );
        }
        else
        {
            var service = new FreelanceSalaryCalculator();
            return service.Calculate(
                request.SalarySigned ?? 0,
                request.IvaIncluded,
                request.IeesSalary ?? 0,
                request.InitDate,
                request.EndDate
            );
        }

    }
}