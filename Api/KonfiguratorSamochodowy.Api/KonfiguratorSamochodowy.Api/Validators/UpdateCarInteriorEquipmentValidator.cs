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
            
            When(x => !string.IsNullOrEmpty(x.Intensity), () => {
                RuleFor(x => x.Intensity)
                    .Must(BeValidIntensity).WithMessage("Intensywność musi być liczbą między 1 a 10");
            });
            
            // ControlType validation removed - we don't have context about equipment type in update request
        }
        
        private bool BeValidIntensity(string intensity)
        {
            if (string.IsNullOrEmpty(intensity))
                return false;
                
            if (int.TryParse(intensity, out var value))
            {
                return value >= 1 && value <= 10;
            }
            
            return false;
        }
    }
}