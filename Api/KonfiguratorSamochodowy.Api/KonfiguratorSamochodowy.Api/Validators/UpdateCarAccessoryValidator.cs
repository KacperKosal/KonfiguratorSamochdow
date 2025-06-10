using FluentValidation;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Validators
{
    public class UpdateCarAccessoryValidator : AbstractValidator<UpdateCarAccessoryRequest>
    {
        private readonly ICarAccessoryRepository _repository;
        
        public UpdateCarAccessoryValidator(ICarAccessoryRepository repository)
        {
            _repository = repository;
            When(x => !string.IsNullOrEmpty(x.Name), () => {
                RuleFor(x => x.Name)
                    .MaximumLength(255).WithMessage("Przekroczono dozwoloną liczbę znaków lub wartość liczbową.");
            });
            
            When(x => !string.IsNullOrEmpty(x.PartNumber), () => {
                RuleFor(x => x.PartNumber)
                    .MaximumLength(255).WithMessage("Przekroczono dozwoloną liczbę znaków lub wartość liczbową.");
                // Uwaga: walidacja unikalności dla Update wymaga dodatkowej logiki w kontrolerze
            });
            
            When(x => !string.IsNullOrEmpty(x.Manufacturer), () => {
                RuleFor(x => x.Manufacturer)
                    .MaximumLength(255).WithMessage("Przekroczono dozwoloną liczbę znaków lub wartość liczbową.");
            });
            
            When(x => !string.IsNullOrEmpty(x.Compatibility), () => {
                RuleFor(x => x.Compatibility)
                    .MaximumLength(255).WithMessage("Przekroczono dozwoloną liczbę znaków lub wartość liczbową.");
            });

            When(x => x.Price.HasValue, () => {
                RuleFor(x => x.Price.Value)
                    .GreaterThanOrEqualTo(0).WithMessage("Cena nie może być ujemna")
                    .LessThanOrEqualTo(1000000).WithMessage("Przekroczono dozwoloną liczbę znaków lub wartość liczbową.");
            });

            When(x => x.StockQuantity.HasValue, () => {
                RuleFor(x => x.StockQuantity.Value)
                    .GreaterThanOrEqualTo(0).WithMessage("Ilość w magazynie nie może być ujemna")
                    .LessThanOrEqualTo(1000000).WithMessage("Przekroczono dozwoloną liczbę znaków lub wartość liczbową.");
            });

            When(x => !string.IsNullOrEmpty(x.Size), () => {
                RuleFor(x => x.Size)
                    .Must(x => new[] { "17", "18", "19", "20" }.Contains(x))
                    .WithMessage("Dozwolone rozmiary felg: 17, 18, 19, 20");
            });

            When(x => x.Capacity.HasValue, () => {
                RuleFor(x => x.Capacity.Value)
                    .GreaterThan(0).WithMessage("Pojemność musi być większa od 0")
                    .LessThanOrEqualTo(1000000).WithMessage("Przekroczono dozwoloną liczbę znaków lub wartość liczbową.");
            });

            When(x => x.MaxLoad.HasValue, () => {
                RuleFor(x => x.MaxLoad.Value)
                    .GreaterThan(0).WithMessage("Maksymalne obciążenie musi być większe od 0")
                    .LessThanOrEqualTo(1000000).WithMessage("Przekroczono dozwoloną liczbę znaków lub wartość liczbową.");
            });

            When(x => !string.IsNullOrEmpty(x.InstallationDifficulty), () => {
                RuleFor(x => x.InstallationDifficulty)
                    .Must(BeValidInstallationDifficulty)
                    .WithMessage("Nieprawidłowy poziom trudności instalacji");
            });
            
            When(x => !string.IsNullOrEmpty(x.Description), () => {
                RuleFor(x => x.Description)
                    .MaximumLength(800).WithMessage("Opis nie może przekraczać 800 znaków");
            });
        }

        private bool BeValidInstallationDifficulty(string difficulty)
        {
            return new[] { "Easy", "Medium", "Professional" }.Contains(difficulty);
        }
    }
}
