using CongestionTaxCalculator.Application.Common.Persistence;
using CongestionTaxCalculator.Domain.City;
using CongestionTaxCalculator.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CongestionTaxCalculator.Infrastructure.Data.Repositories;

public class CityRepository(AppDbContext dbContext) : ICityRepository
{
    public async Task<City?> GetCityByNameAsync(string cityName, CancellationToken cancellationToken = default)
    {
        return await dbContext.Cities.FirstOrDefaultAsync(x => x.Name == cityName, cancellationToken);
    }
}