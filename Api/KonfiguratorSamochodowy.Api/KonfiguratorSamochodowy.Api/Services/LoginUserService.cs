using KonfiguratorSamochodowy.Api.Common.Services;
using KonfiguratorSamochodowy.Api.Enums;
using KonfiguratorSamochodowy.Api.Exceptions;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Requests;
using KonfiguratorSamochodowy.Api.Result;
using KonfiguratorSamochodowy.Api.Validators;
using KonfiguratorSamochodowy.Api.Repositories.Enums;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;

namespace KonfiguratorSamochodowy.Api.Services;

internal class LoginUserService(IUserRepository userRepository, IJwtService jwtService, IConfiguration configuration, ILoggingRepository loggingRepository) : ILoginUserService
{
    public async Task<LoginResult> LoginUserAsync(LoginRequest request, string ipAddress)
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
                        await loggingRepository.LogLoginAttemptAsync(null, ipAddress, Statustyp.Niepowodzenie);
                        throw new LoginRequestInvalidEmail(error.ErrorMessage);
                    case nameof(ValidationErrorCodes.LoginRequestEmailInvalid):
                        await loggingRepository.LogLoginAttemptAsync(null, ipAddress, Statustyp.Niepowodzenie);
                        throw new LoginRequestInvalidEmail(error.ErrorMessage);
                    case nameof(ValidationErrorCodes.LoginRequestInvalidPassword):
                        await loggingRepository.LogLoginAttemptAsync(null, ipAddress, Statustyp.Niepowodzenie);
                        throw new LoginRequestInvalidPassword(error.ErrorCode, error.ErrorMessage);
                }
            }
        }
        var user = await userRepository.GetByEmailAsync(request.Email);
        if (user == null)
        {
            await loggingRepository.LogLoginAttemptAsync(null, ipAddress, Statustyp.Niepowodzenie);
            throw new LoginRequestInvalidEmail("Nie znaleziono użytkownika o podanym adresie email.");
        }
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Haslo))
        {
            await loggingRepository.LogLoginAttemptAsync(user.Id, ipAddress, Statustyp.Niepowodzenie);
            throw new LoginRequestInvalidPassword(nameof(ValidationErrorCodes.LoginRequestInvalidPassword),"Nieprawidłowe hasło.");
        }

        var token = jwtService.GenerateToken(user.Id, user.Rola, user.ImieNazwisko);
        var refreshToken = jwtService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpires = DateTime.UtcNow.AddSeconds(configuration.GetValue<int>("JwtInformations:RefreshTokenExpirationSeconds"));

        await userRepository.UpdateAsync(user);
        await loggingRepository.LogLoginAttemptAsync(user.Id, ipAddress, Statustyp.Sukces);

        return new LoginResult
        {
            Token = token,
            RefreshToken = refreshToken,
        };
    }
}
