namespace DotNetTask.Services.Exceptions;

public class BusinessException : Exception
{
    public Dictionary<string, List<string>> Errors { get; set; } = [];
    public BusinessException() { }

    public BusinessException(string message) : base(message) { }

    public BusinessException(string message, Exception innerException) : base(message, innerException) { }

    public BusinessException(string message, Dictionary<string, List<string>> errors) : base(message)
    {
        Errors = errors;
    }
    public BusinessException(string message, Exception innerException, Dictionary<string, List<string>> errors) : base(message, innerException)
    {
        Errors = errors;
    }
}
