using FluentValidation;
using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Validators
{
    public class UpdateCarAccessoryValidator : AbstractValidator<UpdateCarAccessoryRequest>
    {
        public UpdateCarAccessoryValidator()
        {
            When(x => !string.IsNullOrEmpty(x.Name), () => {
                RuleFor(x => x.Name)
                    .MaximumLength(100).WithMessage("Nazwa nie może być dłuższa niż 100 znaków");
            });

            When(x => x.Price.HasValue, () => {
                RuleFor(x => x.Price.Value)
                    .GreaterThanOrEqualTo(0).WithMessage("Cena nie może być ujemna");
            });

            When(x => x.StockQuantity.HasValue, () => {
                RuleFor(x => x.StockQuantity.Value)
                    .GreaterThanOrEqualTo(0).WithMessage("Ilość w magazynie nie może być ujemna");
            });

            When(x => !string.IsNullOrEmpty(x.Size), () => {
                RuleFor(x => x.Size)
                    .Must(x => new[] { "17", "18", "19", "20" }.Contains(x))
                    .WithMessage("Dozwolone rozmiary felg: 17, 18, 19, 20");
            });

            When(x => x.Capacity.HasValue, () => {
                RuleFor(x => x.Capacity.Value)
                    .GreaterThan(0).WithMessage("Pojemność musi być większa od 0");
            });

            When(x => x.MaxLoad.HasValue, () => {
                RuleFor(x => x.MaxLoad.Value)
                    .GreaterThan(0).WithMessage("Maksymalne obciążenie musi być większe od 0");
            });

            When(x => !string.IsNullOrEmpty(x.InstallationDifficulty), () => {
                RuleFor(x => x.InstallationDifficulty)
                    .Must(BeValidInstallationDifficulty)
                    .WithMessage("Nieprawidłowy poziom trudności instalacji");
            });
        }

        private bool BeValidInstallationDifficulty(string difficulty)
        {
            return new[] { "Easy", "Medium", "Professional" }.Contains(difficulty);
        }
    }
}
