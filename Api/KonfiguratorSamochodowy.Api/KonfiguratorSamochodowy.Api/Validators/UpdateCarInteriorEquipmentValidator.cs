using FluentValidation;
using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Validators
{
    public class UpdateCarInteriorEquipmentValidator : AbstractValidator<UpdateCarInteriorEquipmentRequest>
    {
        public UpdateCarInteriorEquipmentValidator()
        {
            // Walidacja opcjonalnych pól
            When(x => x.AdditionalPrice.HasValue, () => {
                RuleFor(x => x.AdditionalPrice.Value)
                    .GreaterThanOrEqualTo(0).WithMessage("Cena dodatkowa nie może być ujemna");
            });
            
            When(x => !string.IsNullOrEmpty(x.ColorCode), () => {
                RuleFor(x => x.ColorCode)
                    .Matches("^#[0-9A-Fa-f]{6}$").WithMessage("Kod koloru musi być w formacie #RRGGBB");
            });
            
            When(x => x.Intensity.HasValue, () => {
                RuleFor(x => x.Intensity.Value)
                    .InclusiveBetween(1, 10).WithMessage("Intensywność musi być między 1 a 10");
            });
            
            When(x => !string.IsNullOrEmpty(x.ControlType), () => {
                RuleFor(x => x.ControlType)
                    .Must(BeValidCruiseControlType).WithMessage("Nieprawidłowy typ tempomatu. Dozwolone wartości: Standard, Adaptive, None");
            });
        }
        
        private bool BeValidCruiseControlType(string controlType)
        {
            return new[] { "Standard", "Adaptive", "None" }.Contains(controlType);
        }
    }
}