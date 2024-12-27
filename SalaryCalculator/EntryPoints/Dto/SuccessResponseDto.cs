namespace SalaryCalculator.EntryPoints.Dto;

public class SuccessResponseDto ( object? data = null )
{
    public bool State { get; } = true;
    public object? Data { get; } = data;
}