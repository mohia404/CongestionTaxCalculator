using CongestionTaxCalculator.Domain.City.ValueObjects;
using ErrorOr;
using MediatR;

namespace CongestionTaxCalculator.Application.Cities.Commands;

public record CalculateTaxCommand(
    string CityName,
    Vehicle Vehicle,
    DateTime[] DatePassesToll) : IRequest<ErrorOr<int>>;