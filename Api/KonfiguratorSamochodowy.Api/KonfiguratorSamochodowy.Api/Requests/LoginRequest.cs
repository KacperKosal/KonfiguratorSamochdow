﻿namespace KonfiguratorSamochodowy.Api.Requests;

internal class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
