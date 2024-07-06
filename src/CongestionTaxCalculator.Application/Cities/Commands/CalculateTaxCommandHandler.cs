using ErrorOr;
using MediatR;
using CongestionTaxCalculator.Application.Common.Errors;
using CongestionTaxCalculator.Application.Common.Persistence;
using CongestionTaxCalculator.Domain.City.Exceptions;

namespace CongestionTaxCalculator.Application.Cities.Commands;

public class CalculateTaxCommandHandler(ICityRepository repository)
    : IRequestHandler<CalculateTaxCommand, ErrorOr<int>>
{
    public async Task<ErrorOr<int>> Handle(CalculateTaxCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var city = await repository.GetCityByNameAsync(request.CityName, cancellationToken);

            if (city is null)
                return Errors.City.NotFound;

            var tax = city.GetTax(request.Vehicle, request.DatePassesToll);

            return tax;
        }
        catch (InvalidYearException)
        {
            return Errors.City.InvalidYear;
        }
        catch (MoreThanOneYearCalculationException)
        {
            return Errors.City.MoreThanOneYearCalculation;
        }       
        catch (TaxRulesNotFoundException)
        {
            return Errors.City.TaxRulesNotFound;
        }
    }
}