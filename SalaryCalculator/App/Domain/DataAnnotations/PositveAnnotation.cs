using System.ComponentModel.DataAnnotations;

namespace SalaryCalculator.App.Domain.DataAnnotations;

public class PositveAnnotation: ValidationAttribute
{
    public string? FieldName { get; set; }
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        return value switch
        {
            null => ValidationResult.Success,
            int intValue => intValue < 0
                ? new ValidationResult($"{FieldName ?? "Campo"} no puede ser negativo")
                : ValidationResult.Success,
            _ => new ValidationResult($"{FieldName ?? "Campo"} debe ser positivo y no contener decimales")
        };
    }
}