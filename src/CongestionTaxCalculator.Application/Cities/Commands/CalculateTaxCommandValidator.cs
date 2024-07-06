using FluentValidation;

namespace CongestionTaxCalculator.Application.Cities.Commands;

public class CalculateTaxCommandValidator : AbstractValidator<CalculateTaxCommand>
{
    public CalculateTaxCommandValidator()
    {
        RuleFor(x => x.CityName)
            .NotNull();

        RuleFor(x => x.Vehicle)
            .NotNull();
    }
}