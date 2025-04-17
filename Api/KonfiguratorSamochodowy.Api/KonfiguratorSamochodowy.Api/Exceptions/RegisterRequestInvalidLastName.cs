namespace KonfiguratorSamochodowy.Api.Exceptions;

internal sealed class RegisterRequestInvalidLastName: Exception
{
    public RegisterRequestInvalidLastName(string errorCode, string errorMessage) : base($"{{ \"errorCode\": \"{errorCode}\", \"errorMessage\": \"{errorMessage}\"}}")
    {
    }
}

