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
                    .MaximumLength(255).WithMessage("Przekroczono dozwoloną liczbę znaków lub wartość liczbową.");
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
                    .MaximumLength(255).WithMessage("Przekroczono dozwoloną liczbę znaków lub wartość liczbową.");
            });
            
            When(x => x.BasePrice.HasValue, () => {
                RuleFor(x => x.BasePrice.Value)
                    .GreaterThan(0).WithMessage("Cena podstawowa musi być większa od 0")
                    .LessThanOrEqualTo(1000000).WithMessage("Przekroczono dozwoloną liczbę znaków lub wartość liczbową.");
            });
            
            When(x => !string.IsNullOrEmpty(x.Description), () => {
                RuleFor(x => x.Description)
                    .MaximumLength(800).WithMessage("Opis nie może przekraczać 800 znaków");
            });
        }
        
        private bool BeValidBodyType(string bodyType)
        {
            return BodyType.AllTypes.Contains(bodyType);
        }
    }
}