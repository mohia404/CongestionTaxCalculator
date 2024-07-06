using CongestionTaxCalculator.Domain.Common.Models;

namespace CongestionTaxCalculator.Domain.City.ValueObjects;

public class FixedCongestionTaxAmount : ValueObject
{
    private FixedCongestionTaxAmount(TimeOnly fromTime, TimeOnly toTime, int taxAmount)
    {
        FromTime = fromTime;
        ToTime = toTime;
        TaxAmount = taxAmount;
    }

    public static FixedCongestionTaxAmount Create(TimeOnly fromTime, TimeOnly toTime, int taxAmount)
    {
        return new FixedCongestionTaxAmount(fromTime, toTime, taxAmount);
    }

    public TimeOnly FromTime { get; }
    public TimeOnly ToTime { get; }
    public int TaxAmount { get; }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return FromTime;
        yield return ToTime;
    }


#pragma warning disable CS8618
    private FixedCongestionTaxAmount() { }
#pragma warning restore CS8618
}