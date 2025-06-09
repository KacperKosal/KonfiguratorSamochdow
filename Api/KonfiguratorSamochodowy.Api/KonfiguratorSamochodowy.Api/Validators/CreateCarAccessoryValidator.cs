using FluentValidation;
using KonfiguratorSamochodowy.Api.Repositories.Enums;
using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Validators
{
    public class CreateCarAccessoryValidator : AbstractValidator<CreateCarAccessoryRequest>
    {
        public CreateCarAccessoryValidator()
        {
            RuleFor(x => x.CarId)
                .NotEmpty().WithMessage("ID samochodu jest wymagane");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Kategoria jest wymagana")
                .Must(BeValidCategory).WithMessage("Nieprawidłowa kategoria");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Typ akcesorium jest wymagany")
                .Must(BeValidType).WithMessage("Nieprawidłowy typ akcesorium");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nazwa jest wymagana")
                .MaximumLength(100).WithMessage("Nazwa nie może być dłuższa niż 100 znaków");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Cena nie może być ujemna");

            RuleFor(x => x.PartNumber)
                .NotEmpty().When(x => x.IsOriginalBMWPart)
                .WithMessage("Numer części jest wymagany dla oryginalnych części BMW");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Ilość w magazynie nie może być ujemna");

            // Walidacje specyficzne dla typów akcesoriów
            When(x => x.Type == AccessoryType.AlloyWheels, () => {
                RuleFor(x => x.Size)
                    .NotEmpty().WithMessage("Rozmiar felg jest wymagany")
                    .Must(x => new[] { "17", "18", "19", "20" }.Contains(x))
                    .WithMessage("Dozwolone rozmiary felg: 17, 18, 19, 20");

                RuleFor(x => x.Pattern)
                    .NotEmpty().WithMessage("Wzór felg jest wymagany");
            });

            When(x => x.Type == AccessoryType.FloorMats, () => {
                RuleFor(x => x.Material)
                    .NotEmpty().WithMessage("Materiał dywaników jest wymagany")
                    .Must(x => new[] { "Welur", "Guma", "Tworzywo" }.Contains(x))
                    .WithMessage("Nieprawidłowy materiał dywaników");
            });

            When(x => x.Type == AccessoryType.RoofBoxes, () => {
                RuleFor(x => x.Capacity)
                    .NotNull().WithMessage("Pojemność bagażnika dachowego jest wymagana")
                    .GreaterThan(0).WithMessage("Pojemność musi być większa od 0");

                RuleFor(x => x.MaxLoad)
                    .NotNull().WithMessage("Maksymalne obciążenie jest wymagane")
                    .GreaterThan(0).WithMessage("Maksymalne obciążenie musi być większe od 0");
            });

            When(x => x.Type == AccessoryType.ChildSeats, () => {
                RuleFor(x => x.AgeGroup)
                    .NotEmpty().WithMessage("Grupa wiekowa jest wymagana");

                RuleFor(x => x.MaxLoad)
                    .NotNull().WithMessage("Maksymalna waga dziecka jest wymagana")
                    .GreaterThan(0).WithMessage("Maksymalna waga musi być większa od 0");
            });

            RuleFor(x => x.InstallationDifficulty)
                .Must(BeValidInstallationDifficulty)
                .When(x => !string.IsNullOrEmpty(x.InstallationDifficulty))
                .WithMessage("Nieprawidłowy poziom trudności instalacji");
        }

        private bool BeValidCategory(string category)
        {
            return AccessoryCategory.AllCategories.Contains(category);
        }

        private bool BeValidType(string type)
        {
            return AccessoryType.AllTypes.Contains(type);
        }

        private bool BeValidInstallationDifficulty(string difficulty)
        {
            return new[] { "Easy", "Medium", "Professional" }.Contains(difficulty);
        }
    }
}
