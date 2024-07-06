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
                List<Vehicle> taxFreeVehicles = [
                        Vehicle.Create("Emergency vehicle"),
                        Vehicle.Create("Diplomat vehicle"),
                        Vehicle.Create("Motorcycle"),
                        Vehicle.Create("Military vehicle"),
                        Vehicle.Create("Foreign vehicle")
                    ];

                List<FixedCongestionTaxAmount> fixedCongestionTaxAmounts = [
                        FixedCongestionTaxAmount.Create(new TimeOnly(6, 0),new TimeOnly(6, 29), 8),
                        FixedCongestionTaxAmount.Create(new TimeOnly(6, 30),new TimeOnly(6, 59), 13),
                        FixedCongestionTaxAmount.Create(new TimeOnly(7, 0),new TimeOnly(7, 59), 18),
                        FixedCongestionTaxAmount.Create(new TimeOnly(8, 0),new TimeOnly(8, 29), 13),
                        FixedCongestionTaxAmount.Create(new TimeOnly(8, 30),new TimeOnly(14, 59), 8),
                        FixedCongestionTaxAmount.Create(new TimeOnly(15, 0),new TimeOnly(15, 29), 13),
                        FixedCongestionTaxAmount.Create(new TimeOnly(15, 30),new TimeOnly(16, 59), 18),
                        FixedCongestionTaxAmount.Create(new TimeOnly(17, 00),new TimeOnly(17, 59), 13),
                        FixedCongestionTaxAmount.Create(new TimeOnly(18, 00),new TimeOnly(18, 29), 8),
                        FixedCongestionTaxAmount.Create(new TimeOnly(18, 30),new TimeOnly(5, 59), 0)
                    ];

                #endregion

                var taxRules = TaxRulesPerYear.Create(year, taxFreeDays, taxFreeVehicles, fixedCongestionTaxAmounts, 60, 60);
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