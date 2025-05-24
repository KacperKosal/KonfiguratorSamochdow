namespace KonfiguratorSamochodowy.Api.Exceptions;

internal sealed class ChangePasswordInvalidCurrentPassword : Exception
{
    public ChangePasswordInvalidCurrentPassword(string errorCode, string errorMessage) : base($"{{ \"errorCode\": \"{errorCode}\", \"errorMessage\": \"{errorMessage}\"}}") 
    {
    }
}