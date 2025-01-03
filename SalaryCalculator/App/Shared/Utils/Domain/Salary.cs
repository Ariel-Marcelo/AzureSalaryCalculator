namespace SalaryCalculator.App.Shared.Utils.Domain;

public class Salary
{
    public static Dictionary<SalaryKeys, decimal> Freelance = new()
    {
        { SalaryKeys.TaxWorkerPercent, 15m },
        { SalaryKeys.WorkerContributionPercent, 20m }
    };
    
    public static Dictionary<SalaryKeys, decimal> Dependency = new()
    {
        { SalaryKeys.WorkerContributionPercent, 9.45m },
        { SalaryKeys.EmployerContributionPercent, 11.15m },
        { SalaryKeys.Sbu, 475m }
    };
}