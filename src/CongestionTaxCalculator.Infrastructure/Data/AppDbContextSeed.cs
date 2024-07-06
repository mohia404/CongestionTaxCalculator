using CongestionTaxCalculator.Domain.City;
using CongestionTaxCalculator.Domain.City.Entities;
using CongestionTaxCalculator.Domain.City.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PublicHoliday;

namespace CongestionTaxCalculator.Infrastructure.Data;

public class AppDbContextSeed
{
    private DateTime _testDate = DateTime.Now;
    private readonly AppDbContext _context;
    private readonly ILogger<AppDbContextSeed> _logger;

    public AppDbContextSeed(AppDbContext context,
        ILogger<AppDbContextSeed> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync(DateTime testDate, int retry = 0)
    {
        _logger.LogInformation($"Seeding data.");
        _logger.LogInformation($"DbContext Type: {_context.Database.ProviderName}");

        _testDate = testDate;
        var retryForAvailability = retry;
        try
        {
            await _context.Database.MigrateAsync();

            var cityId = CityId.Create(1);

            if (!await _context.Cities.AnyAsync())
            {
                #region ASSIGNMENT
                
                var cityName = "Gothenburg"; // Sweden
                var year = 2013;
                var holidays = new SwedenPublicHoliday().PublicHolidays(2013);
                var dayBeforeHolidays = holidays.Select(x => x.AddDays(-1)).ToArray();
                var weekends = GetWeekendDates(new DateTime(2013, 1, 1), new DateTime(2013, 12, 31));
                var monthOfJuly = GetDatesBetweenTwoDates(new DateTime(2013, 7, 1), new DateTime(2013, 7, 31));

                DateTime[] taxFreeDays = [.. holidays, .. dayBeforeHolidays, .. weekends, .. monthOfJuly];

                #endregion

                var taxRules = TaxRulesPerYear.Create(year, taxFreeDays, 60, 60);
                var city = City.Create(cityId, cityName, [taxRules]);

                await _context.Cities.AddAsync(city);
            }
        }
        catch (Exception ex)
        {
            if (retryForAvailability < 3)
            {
                retryForAvailability++;
                _logger.LogError(ex.Message);
                await SeedAsync(_testDate, retryForAvailability);
            }

            throw;
        }

        await _context.SaveChangesAsync();
    }

    private static List<DateTime> GetWeekendDates(DateTime startDate, DateTime endDate)
    {
        List<DateTime> weekendList = [];
        for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        {
            if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday)
                weekendList.Add(date);
        }

        return weekendList;
    }

    private static List<DateTime> GetDatesBetweenTwoDates(DateTime startDate, DateTime endDate)
    {
        var dates = new List<DateTime>();

        for (var dt = startDate; dt <= endDate; dt = dt.AddDays(1))
        {
            dates.Add(dt);
        }

        return dates;
    }
}