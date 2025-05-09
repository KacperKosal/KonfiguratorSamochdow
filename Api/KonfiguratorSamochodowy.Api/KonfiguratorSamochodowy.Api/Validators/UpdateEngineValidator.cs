using FluentValidation;
using KonfiguratorSamochodowy.Api.Enums;
using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Validators
{
    public class UpdateEngineValidator : AbstractValidator<UpdateEngineRequest>
    {
        public UpdateEngineValidator()
        {
            When(x => !string.IsNullOrEmpty(x.Name), () => {
                RuleFor(x => x.Name)
                    .MaximumLength(100).WithMessage("Nazwa silnika nie może przekraczać 100 znaków");
            });
            
            When(x => !string.IsNullOrEmpty(x.Type), () => {
                RuleFor(x => x.Type)
                    .Must(BeValidEngineType).WithMessage("Nieprawidłowy typ silnika");
            });
            
            When(x => x.Capacity.HasValue, () => {
                RuleFor(x => x.Capacity.Value)
                    .GreaterThan(0).WithMessage("Pojemność silnika musi być większa od 0");
            });
            
            When(x => x.Power.HasValue, () => {
                RuleFor(x => x.Power.Value)
                    .GreaterThan(0).WithMessage("Moc silnika musi być większa od 0");
            });
            
            When(x => x.Torque.HasValue, () => {
                RuleFor(x => x.Torque.Value)
                    .GreaterThan(0).WithMessage("Moment obrotowy musi być większy od 0");
            });
            
            When(x => x.Cylinders.HasValue, () => {
                RuleFor(x => x.Cylinders.Value)
                    .GreaterThan(0).WithMessage("Liczba cylindrów musi być większa od 0");
            });
            
            When(x => x.Gears.HasValue, () => {
                RuleFor(x => x.Gears.Value)
                    .GreaterThan(0).WithMessage("Liczba biegów musi być większa od 0");
            });
            
            When(x => !string.IsNullOrEmpty(x.DriveType), () => {
                RuleFor(x => x.DriveType)
                    .Must(BeValidDriveType).WithMessage("Nieprawidłowy typ napędu");
            });
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