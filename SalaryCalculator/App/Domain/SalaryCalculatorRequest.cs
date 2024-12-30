using System.ComponentModel.DataAnnotations;
using SalaryCalculator.App.Domain.DataAnnotations;

namespace SalaryCalculator.App.Domain
{
    public class SalaryCalculatorRequest
    {
        [Required(ErrorMessage = $"{nameof(SalarySigned)} is Required")]
        [CurrencyAnnotation(FieldName = nameof(SalarySigned))]
        public decimal? SalarySigned { get; set; } = null;


        [CurrencyAnnotation(FieldName = nameof(SalaryBonus))]
        public decimal SalaryBonus { get; set; } = 0;


        [Required(ErrorMessage = $"{nameof(IsDependencyContract)} is Required")]
        public bool? IsDependencyContract { get; set; } = null;


        [CompareInitialDateTo(nameof(EndDate), FieldName = nameof(InitDate))]
        public DateOnly InitDate { get; set; } = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 1);


        public DateOnly EndDate { get; set; } = new DateOnly(DateTime.Now.Year, DateTime.Now.Month,
            DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));


        public bool AccumulatedBenefits { get; set; } = true;

        public bool IvaIncluded { get; set; } = false;


        public decimal IeesSalary { get; set; } = 0;
    }
}