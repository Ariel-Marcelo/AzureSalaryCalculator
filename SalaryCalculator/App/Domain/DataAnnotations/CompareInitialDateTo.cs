using System.ComponentModel.DataAnnotations;
using SalaryCalculator.App.Shared.Exceptions;

namespace SalaryCalculator.App.Domain.DataAnnotations;

public class CompareInitialDateTo(string dateToCompare) : ValidationAttribute
{
    public string? FieldName { get; set; }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var endDateToCompare = (DateOnly)(validationContext.ObjectType.GetProperty(dateToCompare)!
            .GetValue(validationContext.ObjectInstance, null) ?? throw new BindingException($"{dateToCompare} no es una fecha válida"));

        var initialDateProperty = (DateOnly)(value ?? throw new BindingException($"{nameof(value)} no es una fecha válida" ));

        return endDateToCompare <= initialDateProperty 
            ? new ValidationResult($"{FieldName ?? "Campo" } debe ser menor a {dateToCompare}") 
            : ValidationResult.Success;
    }
}