using FluentValidation;
using KonfiguratorSamochodowy.Api.Enums;
using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Validators
{
    public class CreateCarModelValidator : AbstractValidator<CreateCarModelRequest>
    {
        public CreateCarModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nazwa modelu jest wymagana")
                .MaximumLength(100).WithMessage("Nazwa modelu nie może przekraczać 100 znaków");
                
            RuleFor(x => x.ProductionYear)
                .NotEmpty().WithMessage("Rok produkcji jest wymagany")
                .InclusiveBetween(1900, DateTime.UtcNow.Year + 2)
                .WithMessage($"Rok produkcji musi być między 1900 a {DateTime.UtcNow.Year + 2}");
                
            RuleFor(x => x.BodyType)
                .NotEmpty().WithMessage("Typ nadwozia jest wymagany")
                .Must(BeValidBodyType).WithMessage("Nieprawidłowy typ nadwozia");
                
            RuleFor(x => x.Manufacturer)
                .NotEmpty().WithMessage("Producent jest wymagany")
                .MaximumLength(50).WithMessage("Nazwa producenta nie może przekraczać 50 znaków");
                
            RuleFor(x => x.BasePrice)
                .GreaterThan(0).WithMessage("Cena podstawowa musi być większa od 0");
        }
        
        private bool BeValidBodyType(string bodyType)
        {
            return BodyType.AllTypes.Contains(bodyType);
        }
    }
}