namespace DotNetTask.Services.Exceptions;

public class BusinessException : Exception
{
    public Dictionary<string, bool> Fields { get; set; } = [];
    public BusinessException() { }

    public BusinessException(string message) : base(message) { }

    public BusinessException(string message, Exception innerException) : base(message, innerException) { }
    public BusinessException(string message, Dictionary<string, bool> fields) : base(message)
    {
        Fields = fields;
    }
    public BusinessException(string message, Exception innerException, Dictionary<string, bool> fields) : base(message, innerException)
    {
        Fields = fields;
    }
}
