namespace KonfiguratorSamochodowy.Api.Exceptions;

internal sealed class LoginRequestEmailEmpty: Exception
{
    public LoginRequestEmailEmpty(string errorMessage) : base($"{{ \"errorCode\": \"{nameof(LoginRequestEmailEmpty)}\", \"errorMessage\": \"{errorMessage}\"}}")
    {
    }
}

