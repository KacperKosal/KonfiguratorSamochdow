using FluentValidation;
using KonfiguratorSamochodowy.Api.Repositories.Enums;
using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Validators
{
    public class CreateCarInteriorEquipmentValidator : AbstractValidator<CreateCarInteriorEquipmentRequest>
    {
        public CreateCarInteriorEquipmentValidator()
        {
            RuleFor(x => x.CarId)
                .NotEmpty().WithMessage("ID samochodu jest wymagane");
                
            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Typ wyposażenia jest wymagany")
                .Must(BeValidEquipmentType).WithMessage("Nieprawidłowy typ wyposażenia");
                
            RuleFor(x => x.Value)
                .NotEmpty().WithMessage("Wartość jest wymagana");
                
            RuleFor(x => x.AdditionalPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Cena dodatkowa nie może być ujemna");
                
            // Walidacje specyficzne dla typów
            When(x => x.Type == InteriorEquipmentType.SeatColor, () => {
                RuleFor(x => x.ColorCode)
                    .NotEmpty().WithMessage("Kod koloru jest wymagany dla typu SeatColor")
                    .Matches("^#[0-9A-Fa-f]{6}$").WithMessage("Kod koloru musi być w formacie #RRGGBB");
            });
            
            When(x => x.Type == InteriorEquipmentType.AmbientLighting, () => {
                RuleFor(x => x.ColorCode)
                    .NotEmpty().WithMessage("Kod koloru jest wymagany dla typu AmbientLighting")
                    .Matches("^#[0-9A-Fa-f]{6}$").WithMessage("Kod koloru musi być w formacie #RRGGBB");
                    
                RuleFor(x => x.Intensity)
                    .NotNull().WithMessage("Intensywność oświetlenia ambientowego jest wymagana")
                    .InclusiveBetween(1, 10).WithMessage("Intensywność musi być między 1 a 10");
            });
            
            When(x => x.Type == InteriorEquipmentType.RadioType, () => {
                RuleFor(x => x.HasNavigation)
                    .NotNull().WithMessage("Informacja o nawigacji jest wymagana dla typu RadioType");
                    
                RuleFor(x => x.HasPremiumSound)
                    .NotNull().WithMessage("Informacja o premium sound jest wymagana dla typu RadioType");
            });
            
            When(x => x.Type == InteriorEquipmentType.CruiseControl, () => {
                RuleFor(x => x.ControlType)
                    .NotEmpty().WithMessage("Typ tempomatu jest wymagany dla typu CruiseControl")
                    .Must(BeValidCruiseControlType).WithMessage("Nieprawidłowy typ tempomatu. Dozwolone wartości: Standard, Adaptive, None");
            });
        }
        
        private bool BeValidEquipmentType(string type)
        {
            return InteriorEquipmentType.AllTypes.Contains(type);
        }
        
        private bool BeValidCruiseControlType(string controlType)
        {
            return new[] { "Standard", "Adaptive", "None" }.Contains(controlType);
        }
    }
}