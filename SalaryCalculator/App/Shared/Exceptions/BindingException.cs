namespace SalaryCalculator.App.Shared.Exceptions;

[Serializable]
public class BindingException: Exception
{
    public IEnumerable<object?>? Errors { get; set; } = [];
    
    public BindingException() : base() { }

    public BindingException(string message, IEnumerable<object?>? errors = null) : base(message)
    {
        Errors = errors;
    }
    
    public BindingException(string message): base(message) { }
    
    public BindingException(string message, Exception innerException) 
        : base(message, innerException) { }
    
}