﻿using FluentValidation;
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
                .NotEmpty().WithMessage("Nazwa jest wymagana")
                .MaximumLength(49).WithMessage("Nazwa nie może przekraczać 49 znaków");
                
            RuleFor(x => x.AdditionalPrice)
                .GreaterThanOrEqualTo(200).WithMessage("Cena dodatkowa nie może być mniejsza niż 200 zł")
                .LessThanOrEqualTo(1000000).WithMessage("Przekroczono dozwoloną liczbę znaków lub wartość liczbową.");
                
            // Walidacje specyficzne dla typów
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
            
            RuleFor(x => x.Description)
                .MaximumLength(80).WithMessage("Opis nie może przekraczać 80 znaków");
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