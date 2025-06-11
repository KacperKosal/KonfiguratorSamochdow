using KonfiguratorSamochodowy.Api.Common.Services;
using KonfiguratorSamochodowy.Api.Enums;
using KonfiguratorSamochodowy.Api.Exceptions;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Requests;
using KonfiguratorSamochodowy.Api.Result;
using KonfiguratorSamochodowy.Api.Validators;

namespace KonfiguratorSamochodowy.Api.Services;

internal class LoginUserService(IUserRepository userRepository, IJwtService jwtService, IConfiguration configuration) : ILoginUserService
{
    public async Task<LoginResult> LoginUserAsync(LoginRequest request)
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
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Haslo))
        {
            throw new LoginRequestInvalidPassword(nameof(ValidationErrorCodes.LoginRequestInvalidPassword),"Nieprawidłowe hasło.");
        }

        var token = jwtService.GenerateToken(user.Id, user.Rola, user.ImieNazwisko);
        var refreshToken = jwtService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpires = DateTime.UtcNow.AddSeconds(configuration.GetValue<int>("JwtInformations:RefreshTokenExpirationSeconds"));

        await userRepository.UpdateAsync(user);

        return new LoginResult
        {
            Token = token,
            RefreshToken = refreshToken,
        };
    }
}
