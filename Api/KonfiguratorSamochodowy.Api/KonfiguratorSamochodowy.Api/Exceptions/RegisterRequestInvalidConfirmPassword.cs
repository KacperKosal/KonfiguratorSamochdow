namespace KonfiguratorSamochodowy.Api.Exceptions;

internal sealed class RegisterRequestInvalidConfirmPassword: Exception
{
    public RegisterRequestInvalidConfirmPassword(string errorCode, string errorMessage) : base($"{{ \"errorCode\": \"{errorCode}\", \"errorMessage\": \"{errorMessage}\"}}")
    {
    }
}

