﻿using KonfiguratorSamochodowy.Api.Common.Services;
using KonfiguratorSamochodowy.Api.Enums;
using KonfiguratorSamochodowy.Api.Exceptions;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Requests;
using KonfiguratorSamochodowy.Api.Validators;

namespace KonfiguratorSamochodowy.Api.Services;

internal class LoginUserService(IUserRepository userRepository) : ILoginUserService
{
    public async Task LoginUserAsync(LoginRequest request)
    {
        var requestValidator = new LoginRequestValidator();
        var validationResult = requestValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(x => new { x.ErrorCode, x.ErrorMessage })
                .ToList();
            foreach (var error in errors)
            {
                switch (error.ErrorCode)
                {
                    case nameof(ValidationErrorCodes.LoginRequestEmailEmpty):
                        throw new LoginRequestInvalidEmail(error.ErrorMessage);
                    case nameof(ValidationErrorCodes.LoginRequestEmailInvalid):
                        throw new LoginRequestInvalidEmail(error.ErrorMessage);
                    case nameof(ValidationErrorCodes.LoginRequestInvalidPassword):
                        throw new LoginRequestInvalidPassword(error.ErrorCode, error.ErrorMessage);
                }
            }
        }
        var user = await userRepository.GetByEmailAsync(request.Email);
        if (user == null)
        {
            throw new LoginRequestInvalidEmail("Nie znaleziono użytkownika o podanym adresie email.");
        }
        if (user.Haslo != request.Password)
        {
            throw new LoginRequestInvalidPassword(nameof(ValidationErrorCodes.LoginRequestInvalidPassword),"Nieprawidłowe hasło.");
        }
    }
}
