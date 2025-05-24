using KonfiguratorSamochodowy.Api.Common.Services;
using KonfiguratorSamochodowy.Api.Enums;
using KonfiguratorSamochodowy.Api.Exceptions;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Requests;
using KonfiguratorSamochodowy.Api.Validators;

namespace KonfiguratorSamochodowy.Api.Services;

internal class ChangePasswordService(IUserRepository userRepository) : IChangePasswordService
{
    public async Task ChangePasswordAsync(int userId, ChangePasswordRequest request)
    {
        var requestValidator = new ChangePasswordRequestValidator();
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
                    case nameof(ValidationErrorCodes.ChangePasswordCurrentPasswordEmpty):
                        throw new ChangePasswordInvalidCurrentPassword(error.ErrorCode, error.ErrorMessage);
                    case nameof(ValidationErrorCodes.ChangePasswordNewPasswordEmpty):
                        throw new ChangePasswordInvalidNewPassword(error.ErrorCode, error.ErrorMessage);
                    case nameof(ValidationErrorCodes.ChangePasswordNewPasswordLength):
                        throw new ChangePasswordInvalidNewPassword(error.ErrorCode, error.ErrorMessage);
                }
            }
        }
        
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new ChangePasswordInvalidCurrentPassword(
                nameof(ValidationErrorCodes.ChangePasswordInvalidCurrentPassword), 
                "Użytkownik nie został znaleziony.");
        }
        
        if (user.Haslo != request.CurrentPassword)
        {
            throw new ChangePasswordInvalidCurrentPassword(
                nameof(ValidationErrorCodes.ChangePasswordInvalidCurrentPassword), 
                "Nieprawidłowe obecne hasło.");
        }
        
        if (request.CurrentPassword == request.NewPassword)
        {
            throw new ChangePasswordInvalidNewPassword(
                nameof(ValidationErrorCodes.ChangePasswordNewPasswordLength), 
                "Nowe hasło musi być różne od obecnego hasła.");
        }
        
        user.Haslo = request.NewPassword;
        await userRepository.UpdateAsync(user);
    }
}