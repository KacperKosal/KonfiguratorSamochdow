namespace KonfiguratorSamochodowy.Api.Repositories.Helpers;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public Error? Error { get; }

    private Result(bool isSuccess, T value, Error? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value) => new Result<T>(true, value, null);
    public static Result<T> Failure(Error error) => new Result<T>(false, default!, error);
}

/// <summary>
/// Static factory class for creating Result instances
/// </summary>
public static class Result
{
    public static Result<T> Success<T>(T value) => Result<T>.Success(value);
    public static Result<T> Failure<T>(Error error) => Result<T>.Failure(error);
}
