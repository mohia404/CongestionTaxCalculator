using CongestionTaxCalculator.Domain.City.ValueObjects;

namespace CongestionTaxCalculator.Domain.City.Entities;

public class TaxRules
{
    public DateOnly[] TaxFreeDays { get; set; }

    public int MaximumTaxPerDay { get; set; }

    public int SingleChargeDurationMinutes { get; set; }

    public IReadOnlyList<FixedCongestionTaxAmount> FixedCongestionTaxAmounts { get; set; }
    public IReadOnlyList<Vehicle> TaxFreeVehicles { get; set; }

    public bool IsVehicleTaxFree(Vehicle vehicle)
    {
        if(TaxFreeVehicles.Contains(vehicle))
            return true;

        return false;
    }

    public bool IsTaxFreeDay(DateOnly dateTime)
    {
        if (TaxFreeDays.Contains(dateTime))
            return true;

        return false;
    }

    public int GetFixedTimeTaxAmount(TimeOnly time)
    {
        var taxAmount = FixedCongestionTaxAmounts.FirstOrDefault(x => x.FromTime <= time && time <= x.ToTime);

        return taxAmount?.TaxAmount ?? 0;
    }
}