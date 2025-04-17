using KonfiguratorSamochodowy.Api.Common.Services;
using KonfiguratorSamochodowy.Api.Enums;
using KonfiguratorSamochodowy.Api.Exceptions;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;
using KonfiguratorSamochodowy.Api.Requests;
using KonfiguratorSamochodowy.Api.Validators;
namespace KonfiguratorSamochodowy.Api.Service;

internal class UserCreateService(IUserRepository userRepository) : IUserCreateService
{
    public async Task CreateUserAsync(RegisterRequest request)
    {
        var requestValidator = new RegisterRequestValidator();

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
                    case nameof(ValidationErrorCodes.RegisterRequestFirstNameEmpty):
                        throw new RegisterRequestInvalidFirstName(error.ErrorCode, error.ErrorMessage);
                    case nameof(ValidationErrorCodes.RegisterRequestFirstNameLength):
                        throw new RegisterRequestInvalidFirstName(error.ErrorCode, error.ErrorMessage);
                    case nameof(ValidationErrorCodes.RegisterRequestLastNameEmpty):
                        throw new RegisterRequestInvalidLastName(error.ErrorCode, error.ErrorMessage);
                    case nameof(ValidationErrorCodes.RegisterRequestLastNameLength):
                        throw new RegisterRequestInvalidLastName(error.ErrorCode, error.ErrorMessage);
                    case nameof(ValidationErrorCodes.RegisterRequestEmailEmpty):
                        throw new RegisterRequestInvalidEmail(error.ErrorCode, error.ErrorMessage);
                    case nameof(ValidationErrorCodes.RegisterRequestEmailInvalid):
                        throw new RegisterRequestInvalidEmail(error.ErrorCode, error.ErrorMessage);
                    case nameof(ValidationErrorCodes.RegisterRequestPasswordEmpty):
                        throw new RegisterRequestInvalidPassword(error.ErrorCode, error.ErrorMessage);
                    case nameof(ValidationErrorCodes.RegisterRequestPasswordLength):
                        throw new RegisterRequestInvalidPassword(error.ErrorCode, error.ErrorMessage);
                    case nameof(ValidationErrorCodes.RegisterRequestConfirmPasswordNotEqual):
                        throw new RegisterRequestInvalidConfirmPassword(error.ErrorCode, error.ErrorMessage);
                }
            }
        }

        var user = new User
        {
            ImieNazwisko = request.FirstName + " " + request.LastName,
            Email = request.Email,
            Haslo = request.Password,
        };

        var result = await userRepository.GetByEmailAsync(request.Email);
        if (result != null)
        {
            throw new RegisterRequestEmailAlreadyExists( "Użytkownik o podanym adresie email już istnieje.");
        }
        await userRepository.CreateAsync(user);
    }
}
