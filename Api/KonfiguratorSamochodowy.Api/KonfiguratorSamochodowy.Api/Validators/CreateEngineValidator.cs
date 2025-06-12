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
                .MaximumLength(255).WithMessage("Przekroczono dozwoloną liczbę znaków lub wartość liczbową.");
                
            // Pole Type nie jest już używane - usunąć z walidacji
                
            RuleFor(x => x.Capacity)
                .NotNull().WithMessage("Pojemność silnika jest wymagana")
                .Must(BeValidCapacity).WithMessage("Nieprawidłowa pojemność silnika. Dostępne wartości: 1.0L, 1.6L, 1.8L, 1.9L, 2.0L, 2.5L, 3.0L, 4.0L, 5.0L");
                
            // Walidacja liczby cylindrów - wymagana tylko dla silników nieelektrycznych
            When(x => x.FuelType != "Electric", () => {
                RuleFor(x => x.Cylinders)
                    .NotNull().WithMessage("Liczba cylindrów jest wymagana")
                    .GreaterThan(0).WithMessage("Liczba cylindrów musi być większa od 0")
                    .LessThanOrEqualTo(1000000).WithMessage("Przekroczono dozwoloną liczbę znaków lub wartość liczbową.");
            });
            
            RuleFor(x => x.Power)
                .GreaterThanOrEqualTo(70).WithMessage("Moc silnika musi być co najmniej 70 KM")
                .LessThanOrEqualTo(5000).WithMessage("Moc silnika nie może przekraczać 5000 KM");
                
            RuleFor(x => x.Torque)
                .GreaterThanOrEqualTo(50).WithMessage("Moment obrotowy musi być co najmniej 50 Nm")
                .LessThanOrEqualTo(10000).WithMessage("Moment obrotowy nie może przekraczać 10000 Nm");
                
            RuleFor(x => x.FuelType)
                .NotEmpty().WithMessage("Typ paliwa jest wymagany")
                .MaximumLength(255).WithMessage("Przekroczono dozwoloną liczbę znaków lub wartość liczbową.");
                
            // Walidacja skrzyni biegów - wymagana tylko dla silników nieelektrycznych
            When(x => x.FuelType != "Electric", () => {
                RuleFor(x => x.Transmission)
                    .NotEmpty().WithMessage("Rodzaj skrzyni biegów jest wymagany")
                    .MaximumLength(255).WithMessage("Przekroczono dozwoloną liczbę znaków lub wartość liczbową.");
            });
                
            // Walidacja liczby biegów - wymagana tylko dla silników nieelektrycznych
            When(x => x.FuelType != "Electric", () => {
                RuleFor(x => x.Gears)
                    .GreaterThan(0).WithMessage("Liczba biegów musi być większa od 0")
                    .LessThanOrEqualTo(1000000).WithMessage("Przekroczono dozwoloną liczbę znaków lub wartość liczbową.");
            });
                
            RuleFor(x => x.DriveType)
                .NotEmpty().WithMessage("Typ napędu jest wymagany")
                .Must(BeValidDriveType).WithMessage("Nieprawidłowy typ napędu");
                
            // Walidacja zużycia paliwa - wymagana tylko dla silników nieelektrycznych
            When(x => x.FuelType != "Electric", () => {
                RuleFor(x => x.FuelConsumption)
                    .GreaterThanOrEqualTo(0).WithMessage("Zużycie paliwa nie może być ujemne")
                    .LessThanOrEqualTo(500).WithMessage("Zużycie paliwa nie może przekraczać 500 l/100km");
            });
            
            // Walidacja emisji CO2 - wymagana tylko dla silników nieelektrycznych
            When(x => x.FuelType != "Electric", () => {
                RuleFor(x => x.CO2Emission)
                    .GreaterThanOrEqualTo(0).WithMessage("Emisja CO2 nie może być ujemna")
                    .LessThanOrEqualTo(100).WithMessage("Emisja CO2 nie może przekraczać 100 g/km");
            });
                
            RuleFor(x => x.Description)
                .MaximumLength(800).WithMessage("Opis nie może przekraczać 800 znaków");
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