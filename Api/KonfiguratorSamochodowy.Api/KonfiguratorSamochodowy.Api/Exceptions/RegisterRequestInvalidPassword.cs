namespace KonfiguratorSamochodowy.Api.Exceptions;

internal sealed class RegisterRequestInvalidPassword: Exception
{
    public RegisterRequestInvalidPassword(string errorCode, string errorMessage) : base($"{{ \"errorCode\": \"{errorCode}\", \"errorMessage\": \"{errorMessage}\"}}")
    {
    }
}

