using FluentValidation;
using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Validators
{
    public class UpdateCarModelEngineValidator : AbstractValidator<UpdateCarModelEngineRequest>
    {
        public UpdateCarModelEngineValidator()
        {
            When(x => x.AdditionalPrice.HasValue, () => {
                RuleFor(x => x.AdditionalPrice.Value)
                    .GreaterThanOrEqualTo(0).WithMessage("Dodatkowa cena nie może być ujemna");
            });
            
            When(x => x.TopSpeed.HasValue, () => {
                RuleFor(x => x.TopSpeed.Value)
                    .GreaterThan(0).WithMessage("Prędkość maksymalna musi być większa od 0");
            });
            
            When(x => x.Acceleration0To100.HasValue, () => {
                RuleFor(x => x.Acceleration0To100.Value)
                    .GreaterThan(0).WithMessage("Przyspieszenie 0-100 musi być większe od 0");
            });
        }
    }
}