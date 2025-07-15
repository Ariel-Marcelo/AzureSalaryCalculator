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

        [CurrencyAnnotation(FieldName = nameof(IeesSalary))]
        [Range(475.0, (double)decimal.MaxValue, ErrorMessage = $"{nameof(IeesSalary)} debe ser mayor a 475")]
        public decimal? IeesSalary { get; set; } = null;

        public decimal Utilities { get; set; } = 0;
        
        public bool HasReserveFunds { get; set; } = false;
    }
}