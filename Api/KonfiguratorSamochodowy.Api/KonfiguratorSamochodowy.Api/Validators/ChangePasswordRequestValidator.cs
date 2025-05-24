using FluentValidation;
using KonfiguratorSamochodowy.Api.Enums;
using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Validators;

internal sealed class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .WithErrorCode(nameof(ValidationErrorCodes.ChangePasswordCurrentPasswordEmpty))
            .WithMessage("Obecne hasło jest wymagane.");
        
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithErrorCode(nameof(ValidationErrorCodes.ChangePasswordNewPasswordEmpty))
            .WithMessage("Nowe hasło jest wymagane.")
            .MinimumLength(6)
            .WithErrorCode(nameof(ValidationErrorCodes.ChangePasswordNewPasswordLength))
            .WithMessage("Nowe hasło musi zawierać co najmniej 6 znaków.");
    }
}