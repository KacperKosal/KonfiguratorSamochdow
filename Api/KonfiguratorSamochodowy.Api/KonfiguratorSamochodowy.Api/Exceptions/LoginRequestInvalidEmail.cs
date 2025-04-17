namespace KonfiguratorSamochodowy.Api.Exceptions;

internal sealed class LoginRequestInvalidEmail: Exception
{
    public LoginRequestInvalidEmail(string errorMessage) : base($"{{ \"errorCode\": \"{nameof(LoginRequestInvalidEmail)}\", \"errorMessage\": \"{errorMessage}\"}}")
    {
    }
}
