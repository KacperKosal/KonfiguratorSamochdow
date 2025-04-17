namespace KonfiguratorSamochodowy.Api.Exceptions;

internal sealed class RegisterRequestInvalidEmail: Exception
{
    public RegisterRequestInvalidEmail(string errorCode, string errorMessage) : base($"{{ \"errorCode\": \"{errorCode}\", \"errorMessage\": \"{errorMessage}\"}}")
    {
    }
}

