using CongestionTaxCalculator.Domain.Common.Models;

namespace CongestionTaxCalculator.Domain.City.ValueObjects;

public class FixedCongestionTaxAmount : ValueObject
{
    public TimeOnly FromTime { get; set; }
    public TimeOnly ToTime { get; set; }
    public int TaxAmount { get; set; }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return FromTime;
        yield return ToTime;
    }
}