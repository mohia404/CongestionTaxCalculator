using CongestionTaxCalculator.Domain.City;

namespace CongestionTaxCalculator.Application.Common.Persistence;

public interface ICityRepository
{
    public Task<City?> GetCityByNameAsync(string cityName, CancellationToken cancellationToken = default);
}