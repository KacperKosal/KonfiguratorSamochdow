﻿namespace KonfiguratorSamochodowy.Api.Repositories.Helpers;

public class Error
{
    public string Code { get; }
    public string Message { get; }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }
}