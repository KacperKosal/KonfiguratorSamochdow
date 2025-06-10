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
                .MaximumLength(255).WithMessage("Przekroczono dozwoloną liczbę znaków lub wartość liczbową.");
                
            RuleFor(x => x.ProductionYear)
                .NotEmpty().WithMessage("Rok produkcji jest wymagany")
                .InclusiveBetween(1900, DateTime.UtcNow.Year + 2)
                .WithMessage($"Rok produkcji musi być między 1900 a {DateTime.UtcNow.Year + 2}");
                
            RuleFor(x => x.BodyType)
                .NotEmpty().WithMessage("Typ nadwozia jest wymagany")
                .Must(BeValidBodyType).WithMessage("Nieprawidłowy typ nadwozia");
                
            RuleFor(x => x.Manufacturer)
                .NotEmpty().WithMessage("Producent jest wymagany")
                .MaximumLength(255).WithMessage("Przekroczono dozwoloną liczbę znaków lub wartość liczbową.");
                
            RuleFor(x => x.BasePrice)
                .GreaterThan(0).WithMessage("Cena podstawowa musi być większa od 0")
                .LessThanOrEqualTo(1000000).WithMessage("Przekroczono dozwoloną liczbę znaków lub wartość liczbową.");
                
            RuleFor(x => x.Description)
                .MaximumLength(800).WithMessage("Opis nie może przekraczać 800 znaków");
        }
        
        private bool BeValidBodyType(string bodyType)
        {
            return BodyType.AllTypes.Contains(bodyType);
        }
    }
}