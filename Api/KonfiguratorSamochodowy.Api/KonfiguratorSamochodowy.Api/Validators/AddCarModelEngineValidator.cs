using FluentValidation;
using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Validators
{
    public class AddCarModelEngineValidator : AbstractValidator<AddCarModelEngineRequest>
    {
        public AddCarModelEngineValidator()
        {
            RuleFor(x => x.EngineId)
                .NotEmpty().WithMessage("ID silnika jest wymagane");
                
            RuleFor(x => x.AdditionalPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Dodatkowa cena nie może być ujemna");
                
            RuleFor(x => x.TopSpeed)
                .GreaterThan(0).WithMessage("Prędkość maksymalna musi być większa od 0");
                
            RuleFor(x => x.Acceleration0To100)
                .GreaterThan(0).WithMessage("Przyspieszenie 0-100 musi być większe od 0");
        }
    }
}