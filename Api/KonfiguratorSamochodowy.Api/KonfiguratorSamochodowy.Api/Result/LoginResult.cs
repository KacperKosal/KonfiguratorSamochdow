namespace KonfiguratorSamochodowy.Api.Result;

internal class LoginResult
{
    public string Token { get; set; } = null!;
    public string RefreshToken { get; set; }= null!;
}
