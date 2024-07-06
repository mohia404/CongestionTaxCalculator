using CongestionTaxCalculator.Domain.Common.Models;

namespace CongestionTaxCalculator.Domain.City.ValueObjects;

public sealed class CityId : AggregateRootId<int>
{
    public override int Value { get; protected set; }

    private CityId(int value)
    {
        Value = value;
    }

    public static CityId Create(int value)
    {
        return new CityId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    private CityId() { }
}