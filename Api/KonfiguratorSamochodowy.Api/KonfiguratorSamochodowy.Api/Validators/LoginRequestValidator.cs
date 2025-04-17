using FluentValidation;
using KonfiguratorSamochodowy.Api.Enums;
using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Validators;

internal sealed class LoginRequestValidator: AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithErrorCode(nameof(ValidationErrorCodes.LoginRequestEmailEmpty))
            .WithMessage("Email jest wymagany.")
            .EmailAddress()
            .WithErrorCode(nameof(ValidationErrorCodes.LoginRequestEmailInvalid))
            .WithMessage("Nieprawidłowy format adresu email.");
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithErrorCode(nameof(ValidationErrorCodes.LoginRequestInvalidPassword))
            .WithMessage("Hasło jest wymagane.");
    }
}

