using CongestionTaxCalculator.Application.Cities.Commands;
using CongestionTaxCalculator.Contracts.Cities;
using CongestionTaxCalculator.Domain.City.ValueObjects;
using CongestionTaxCalculator.Domain.Common.Exceptions;
using MediatR;

namespace CongestionTaxCalculator.Api.Endpoints;

public static class CityEndpoints
{
    public static void MapCityEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/cities")
            .WithOpenApi();

        group.MapPost("calculate-tax", CalculateTax);
    }

    public static async Task<IResult> CalculateTax(CalculateTaxRequest request, ISender sender, CancellationToken cancellationToken)
    {
        try
        {
            var command = new CalculateTaxCommand(request.CityName, Vehicle.Create(request.VehicleName), request.DatePassesToll);

            var result = await sender.Send(command, cancellationToken);

            return result.Match(
                value => Results.Ok(new CalculateTaxResponse(value)),
                ErrorExtensions.ToProblemResult);
        }
        catch (InvalidDataException ex)
        {

            throw;
        }
    }
}