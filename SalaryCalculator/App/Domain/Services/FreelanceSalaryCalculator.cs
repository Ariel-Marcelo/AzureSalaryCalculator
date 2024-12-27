namespace SalaryCalculator.App.Domain.Services;

public class FreelanceSalaryCalculator: SalaryCalculator
{
    private const decimal TAX_PERCENT = 15;
    private const decimal TAX_WORKER = 20m;

    public SalaryEstimation Calculate(
        decimal salarySigned,
        bool taxIncluded,
        decimal ieesSalary,
        DateOnly initialDate,
        DateOnly finalDate
    )
    {
        var taxIees = ieesSalary * (TAX_WORKER / 100m);
        var taxReverse = taxIncluded ? 100 /(100 + TAX_PERCENT) : 1;
        var liquidityPerMonthAmount = salarySigned * taxReverse;
        var liquidityAmountInTime = GetTotalAmountInTime(initialDate, finalDate, liquidityPerMonthAmount);
        
        return SalaryEstimation.CreateForFreelanceContract(
            taxIncluded ? salarySigned : liquidityPerMonthAmount * ( (100 + TAX_PERCENT) / 100m),
            liquidityPerMonthAmount - taxIees,
            liquidityAmountInTime * ( (100 + TAX_PERCENT) / 100m),
            liquidityAmountInTime,
            [
                new { services = taxIncluded 
                        ? -(salarySigned - liquidityPerMonthAmount) 
                        : -(salarySigned * TAX_PERCENT / 100) }
            ],
            ieesSalary,
            initialDate,
            finalDate
        );
    }
}