using FluentValidation;
using KonfiguratorSamochodowy.Api.Enums;
using KonfiguratorSamochodowy.Api.Requests;
namespace KonfiguratorSamochodowy.Api.Validators;

internal sealed class RegisterRequestValidator: AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithErrorCode(nameof(ValidationErrorCodes.RegisterRequestFirstNameEmpty))
            .WithMessage("Imię jest wymagane.")
            .Length(2, 50)
            .WithErrorCode(nameof(ValidationErrorCodes.RegisterRequestFirstNameLength))
            .WithMessage("Imię musi mieć od 2 do 50 znaków.");
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithErrorCode(nameof(ValidationErrorCodes.RegisterRequestLastNameEmpty))
            .WithMessage("Nazwisko jest wymagane.")
            .Length(2, 50)
            .WithErrorCode(nameof(ValidationErrorCodes.RegisterRequestLastNameLength))
            .WithMessage("Nazwisko musi mieć od 2 do 50 znaków.");
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithErrorCode(nameof(ValidationErrorCodes.RegisterRequestEmailEmpty))
            .WithMessage("Email jest wymagany.")
            .EmailAddress()
            .WithErrorCode(nameof(ValidationErrorCodes.RegisterRequestEmailInvalid))
            .WithMessage("Nieprawidłowy format adresu email.");
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithErrorCode(nameof(ValidationErrorCodes.RegisterRequestPasswordEmpty))
            .WithMessage("Hasło jest wymagane.")
            .MinimumLength(8)
            .WithErrorCode(nameof(ValidationErrorCodes.RegisterRequestPasswordLength))
            .WithMessage("Hasło musi mieć co najmniej 8 znaków.");
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password)
            .WithErrorCode(nameof(ValidationErrorCodes.RegisterRequestConfirmPasswordNotEqual))
            .WithMessage("Hasła muszą być takie same.");
    }
}

