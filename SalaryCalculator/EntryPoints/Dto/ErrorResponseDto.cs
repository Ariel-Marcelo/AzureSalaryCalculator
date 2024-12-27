namespace SalaryCalculator.EntryPoints.Dto;

public class ErrorResponseDto(string message, object? errors = null)
{
    public bool State { get; } = false;
    public string Message { get; } = message;
    public object? Errors { get; }  = errors;
}