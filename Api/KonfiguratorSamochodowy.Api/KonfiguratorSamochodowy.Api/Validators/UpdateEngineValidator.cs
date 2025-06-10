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
                    .MaximumLength(255).WithMessage("Przekroczono dozwoloną liczbę znaków lub wartość liczbową.");
            });
            
            When(x => !string.IsNullOrEmpty(x.Type), () => {
                RuleFor(x => x.Type)
                    .Must(BeValidEngineType).WithMessage("Nieprawidłowy typ silnika");
            });
            
            When(x => x.Capacity.HasValue, () => {
                RuleFor(x => x.Capacity)
                    .Must(BeValidCapacity).WithMessage("Nieprawidłowa pojemność silnika. Dostępne wartości: 1.0L, 1.6L, 1.8L, 1.9L, 2.0L, 2.5L, 3.0L, 4.0L, 5.0L");
            });
            
            When(x => x.Power.HasValue, () => {
                RuleFor(x => x.Power.Value)
                    .GreaterThan(0).WithMessage("Moc silnika musi być większa od 0")
                    .LessThanOrEqualTo(1000000).WithMessage("Przekroczono dozwoloną liczbę znaków lub wartość liczbową.");
            });
            
            When(x => x.Torque.HasValue, () => {
                RuleFor(x => x.Torque.Value)
                    .GreaterThan(0).WithMessage("Moment obrotowy musi być większy od 0")
                    .LessThanOrEqualTo(1000000).WithMessage("Przekroczono dozwoloną liczbę znaków lub wartość liczbową.");
            });
            
            When(x => x.Cylinders.HasValue, () => {
                RuleFor(x => x.Cylinders.Value)
                    .GreaterThan(0).WithMessage("Liczba cylindrów musi być większa od 0")
                    .LessThanOrEqualTo(1000000).WithMessage("Przekroczono dozwoloną liczbę znaków lub wartość liczbową.");
            });
            
            When(x => x.Gears.HasValue, () => {
                RuleFor(x => x.Gears.Value)
                    .GreaterThan(0).WithMessage("Liczba biegów musi być większa od 0")
                    .LessThanOrEqualTo(1000000).WithMessage("Przekroczono dozwoloną liczbę znaków lub wartość liczbową.");
            });
            
            When(x => !string.IsNullOrEmpty(x.DriveType), () => {
                RuleFor(x => x.DriveType)
                    .Must(BeValidDriveType).WithMessage("Nieprawidłowy typ napędu");
            });
            
            When(x => !string.IsNullOrEmpty(x.Description), () => {
                RuleFor(x => x.Description)
                    .MaximumLength(800).WithMessage("Opis nie może przekraczać 800 znaków");
            });
        }
        
        private bool BeValidEngineType(string engineType)
        {
            return EngineType.AllTypes.Contains(engineType);
        }
        
        private bool BeValidDriveType(string driveType)
        {
            return KonfiguratorSamochodowy.Api.Enums.DriveType.AllTypes.Contains(driveType);
        }
        
        private bool BeValidCapacity(int? capacity)
        {
            if (!capacity.HasValue) return false;
            var allowedCapacities = new[] { 1000, 1600, 1800, 1900, 2000, 2500, 3000, 4000, 5000 };
            return allowedCapacities.Contains(capacity.Value);
        }
    }
}