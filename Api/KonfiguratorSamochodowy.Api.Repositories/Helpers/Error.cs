namespace KonfiguratorSamochodowy.Api.Repositories.Helpers;

public class Error
{
    public string Code { get; }
    public string Message { get; }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    /// <summary>
    /// Static factory methods for creating Error instances
    /// </summary>
    public static Error Failure(string code, string message) => new Error(code, message);
    public static Error NotFound(string code, string message) => new Error(code, message);
    public static Error Validation(string code, string message) => new Error(code, message);
    public static Error Unauthorized(string code, string message) => new Error(code, message);
}