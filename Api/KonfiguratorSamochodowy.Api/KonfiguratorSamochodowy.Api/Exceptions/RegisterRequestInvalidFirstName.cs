namespace KonfiguratorSamochodowy.Api.Exceptions;

internal sealed class RegisterRequestInvalidFirstName: Exception
{
    public RegisterRequestInvalidFirstName(string errorCode, string errorMessage) : base($"{{ \"errorCode\": \"{errorCode}\", \"errorMessage\": \"{errorMessage}\"}}")
    {
    }
}
