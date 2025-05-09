using FluentValidation;
using KonfiguratorSamochodowy.Api.Enums;
using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Validators
{
    public class CreateEngineValidator : AbstractValidator<CreateEngineRequest>
    {
        public CreateEngineValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nazwa silnika jest wymagana")
                .MaximumLength(100).WithMessage("Nazwa silnika nie może przekraczać 100 znaków");
                
            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Typ silnika jest wymagany")
                .Must(BeValidEngineType).WithMessage("Nieprawidłowy typ silnika");
                
            When(x => x.Type != EngineType.Electric, () => {
                RuleFor(x => x.Capacity)
                    .NotNull().WithMessage("Pojemność silnika jest wymagana dla silników nieelektrycznych")
                    .GreaterThan(0).WithMessage("Pojemność silnika musi być większa od 0");
                    
                RuleFor(x => x.Cylinders)
                    .NotNull().WithMessage("Liczba cylindrów jest wymagana dla silników nieelektrycznych")
                    .GreaterThan(0).WithMessage("Liczba cylindrów musi być większa od 0");
            });
            
            RuleFor(x => x.Power)
                .GreaterThan(0).WithMessage("Moc silnika musi być większa od 0");
                
            RuleFor(x => x.Torque)
                .GreaterThan(0).WithMessage("Moment obrotowy musi być większy od 0");
                
            RuleFor(x => x.FuelType)
                .NotEmpty().WithMessage("Typ paliwa jest wymagany");
                
            RuleFor(x => x.Transmission)
                .NotEmpty().WithMessage("Rodzaj skrzyni biegów jest wymagany");
                
            RuleFor(x => x.Gears)
                .GreaterThan(0).WithMessage("Liczba biegów musi być większa od 0");
                
            RuleFor(x => x.DriveType)
                .NotEmpty().WithMessage("Typ napędu jest wymagany")
                .Must(BeValidDriveType).WithMessage("Nieprawidłowy typ napędu");
        }
        
        private bool BeValidEngineType(string engineType)
        {
            return EngineType.AllTypes.Contains(engineType);
        }
        
        private bool BeValidDriveType(string driveType)
        {
            return DriveType.AllTypes.Contains(driveType);
        }
    }
}