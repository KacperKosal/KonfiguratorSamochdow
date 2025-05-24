namespace KonfiguratorSamochodowy.Api.Exceptions;

internal sealed class ChangePasswordInvalidNewPassword : Exception
{
    public ChangePasswordInvalidNewPassword(string errorCode, string errorMessage) : base($"{{ \"errorCode\": \"{errorCode}\", \"errorMessage\": \"{errorMessage}\"}}") 
    {
    }
}