using ErrorOr;
using MediatR;
using CongestionTaxCalculator.Application.Common.Errors;
using CongestionTaxCalculator.Application.Common.Persistence;

namespace CongestionTaxCalculator.Application.Cities.Commands;

public class CalculateTaxCommandHandler(ICityRepository repository)
    : IRequestHandler<CalculateTaxCommand, ErrorOr<int>>
{
    public async Task<ErrorOr<int>> Handle(CalculateTaxCommand request, CancellationToken cancellationToken)
    {
        var city = await repository.GetCityByNameAsync(request.CityName, cancellationToken);

        if (city is null)
            return Errors.City.NotFound;

        var tax = city.GetTax(request.Vehicle, request.DatePassesToll);

        return tax;
    }
}