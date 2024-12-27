using System.ComponentModel.DataAnnotations;

namespace SalaryCalculator.App.Domain.DataAnnotations;

public class CurrencyAnnotation: ValidationAttribute
{
    public string? FieldName { get; set; }
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        return value switch
        {
            null => ValidationResult.Success,
            
            decimal and < 0 => new ValidationResult(
                $"{FieldName ?? "Campo"} no puede ser negativo"),
            
            decimal decimalValue => decimal.Round(decimalValue, 2) != decimalValue
                ? new ValidationResult($"{FieldName ?? "Campo"} debe tener máximo 2 decimales")
                : ValidationResult.Success,
            
            _ => new ValidationResult($"{FieldName ?? "Campo"} debe ser un número decimal")
        };
    }
}