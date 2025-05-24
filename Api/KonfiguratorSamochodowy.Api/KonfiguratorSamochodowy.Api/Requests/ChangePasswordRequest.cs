namespace KonfiguratorSamochodowy.Api.Requests;

internal class ChangePasswordRequest
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}