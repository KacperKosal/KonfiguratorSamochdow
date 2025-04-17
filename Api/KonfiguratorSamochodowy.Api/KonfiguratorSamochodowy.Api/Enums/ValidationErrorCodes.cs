namespace KonfiguratorSamochodowy.Api.Enums;

internal enum ValidationErrorCodes
{
    Unknown = 0,
    RegisterRequestFirstNameEmpty = 1,
    RegisterRequestFirstNameLength = 2,
    RegisterRequestLastNameEmpty = 3,
    RegisterRequestLastNameLength = 4,
    RegisterRequestEmailEmpty = 5,
    RegisterRequestEmailInvalid = 6,
    RegisterRequestPasswordEmpty = 7,
    RegisterRequestPasswordLength = 8,
    RegisterRequestConfirmPasswordNotEqual = 9,
    RegisterRequestEmailAlreadyExists = 10,
    LoginRequestEmailEmpty = 11,
    LoginRequestEmailInvalid = 12,
    LoginRequestInvalidPassword = 13,
    LoginRequestInvalidEmail = 14,

}
