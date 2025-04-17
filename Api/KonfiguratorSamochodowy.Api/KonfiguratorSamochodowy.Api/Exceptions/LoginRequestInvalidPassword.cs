namespace KonfiguratorSamochodowy.Api.Exceptions;

internal sealed class LoginRequestInvalidPassword: Exception
{
    public LoginRequestInvalidPassword(string errorCode, string errorMessage) : base($"{{ \"errorCode\": \"{errorCode}\", \"errorMessage\": \"{errorMessage}\"}}")
    {
    }
}
