using FluentValidation;
using KonfiguratorSamochodowy.Api.Enums;
using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Validators
{
    public class UpdateCarModelValidator : AbstractValidator<UpdateCarModelRequest>
    {
        public UpdateCarModelValidator()
        {
            When(x => !string.IsNullOrEmpty(x.Name), () => {
                RuleFor(x => x.Name)
                    .MaximumLength(100).WithMessage("Nazwa modelu nie może przekraczać 100 znaków");
            });
            
            When(x => x.ProductionYear.HasValue, () => {
                RuleFor(x => x.ProductionYear.Value)
                    .InclusiveBetween(1900, DateTime.UtcNow.Year + 2)
                    .WithMessage($"Rok produkcji musi być między 1900 a {DateTime.UtcNow.Year + 2}");
            });
            
            When(x => !string.IsNullOrEmpty(x.BodyType), () => {
                RuleFor(x => x.BodyType)
                    .Must(BeValidBodyType).WithMessage("Nieprawidłowy typ nadwozia");
            });
            
            When(x => !string.IsNullOrEmpty(x.Manufacturer), () => {
                RuleFor(x => x.Manufacturer)
                    .MaximumLength(50).WithMessage("Nazwa producenta nie może przekraczać 50 znaków");
            });
            
            When(x => x.BasePrice.HasValue, () => {
                RuleFor(x => x.BasePrice.Value)
                    .GreaterThan(0).WithMessage("Cena podstawowa musi być większa od 0");
            });
        }
        
        private bool BeValidBodyType(string bodyType)
        {
            return BodyType.AllTypes.Contains(bodyType);
        }
    }
}