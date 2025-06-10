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
                    .GreaterThanOrEqualTo(0).WithMessage("Cena dodatkowa nie może być ujemna")
                    .LessThanOrEqualTo(1000000).WithMessage("Przekroczono dozwoloną liczbę znaków lub wartość liczbową.");
            });
            
            // ControlType validation removed - we don't have context about equipment type in update request
            
            When(x => !string.IsNullOrEmpty(x.Description), () => {
                RuleFor(x => x.Description)
                    .MaximumLength(800).WithMessage("Opis nie może przekraczać 800 znaków");
            });
        }
    }
}