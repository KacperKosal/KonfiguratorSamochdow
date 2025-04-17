namespace KonfiguratorSamochodowy.Api.Exceptions;

internal sealed class RegisterRequestEmailAlreadyExists: Exception
{
    public RegisterRequestEmailAlreadyExists(string errorMessage) : base($"{{ \"errorCode\": \"{nameof(RegisterRequestEmailAlreadyExists)}\", \"errorMessage\": \"{errorMessage}\"}}")
    {
    }
}
