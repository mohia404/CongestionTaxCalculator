using CongestionTaxCalculator.Domain.City.ValueObjects;
using CongestionTaxCalculator.Domain.Common.Models;

namespace CongestionTaxCalculator.Domain.City.Entities;

public class TaxRulesPerYear : Entity<TaxRulesId>
{
    public int Year { get; }

    public DateTime[] TaxFreeDays { get; }

    public int MaximumTaxPerDay { get; }

    public int SingleChargeDurationMinutes { get; }

    private readonly List<FixedCongestionTaxAmount> fixedCongestionTaxAmounts = [];
    public IReadOnlyList<FixedCongestionTaxAmount> FixedCongestionTaxAmounts => fixedCongestionTaxAmounts.AsReadOnly();

    private readonly List<Vehicle> taxFreeVehicles = [];
    public IReadOnlyList<Vehicle> TaxFreeVehicles => taxFreeVehicles.AsReadOnly();

    private TaxRulesPerYear(TaxRulesId id, int year, DateTime[] taxFreeDays, List<Vehicle> taxFreeVehicles, List<FixedCongestionTaxAmount> fixedCongestionTaxAmounts, int maximumTaxPerDay, int singleChargeDurationMinutes) : base(id)
    {
        Year = year;
        TaxFreeDays = taxFreeDays;
        this.taxFreeVehicles = taxFreeVehicles;
        this.fixedCongestionTaxAmounts = fixedCongestionTaxAmounts;
        MaximumTaxPerDay = maximumTaxPerDay;
        SingleChargeDurationMinutes = singleChargeDurationMinutes;
    }

    public static TaxRulesPerYear Create(int year, DateTime[] taxFreeDays, List<Vehicle> taxFreeVehicles, List<FixedCongestionTaxAmount> fixedCongestionTaxAmounts, int maximumTaxPerDay, int singleChargeDurationMinutes)
    {
        return new TaxRulesPerYear(TaxRulesId.CreateUnique(), year, taxFreeDays, taxFreeVehicles, fixedCongestionTaxAmounts, maximumTaxPerDay, singleChargeDurationMinutes);
    }

    public bool IsVehicleTaxFree(Vehicle vehicle)
    {
        if (TaxFreeVehicles.Contains(vehicle))
            return true;

        return false;
    }

    public bool IsTaxFreeDay(DateTime dateTime)
    {
        if (TaxFreeDays.Any(x=>x <= dateTime && dateTime < x.AddDays(1)))
            return true;

        return false;
    }

    public int GetFixedTimeTaxAmount(TimeOnly time)
    {
        var taxAmount = FixedCongestionTaxAmounts.FirstOrDefault(x => x.FromTime <= time && time <= x.ToTime);

        return taxAmount?.TaxAmount ?? 0;
    }

#pragma warning disable CS8618
    private TaxRulesPerYear() { }
#pragma warning restore CS8618
}