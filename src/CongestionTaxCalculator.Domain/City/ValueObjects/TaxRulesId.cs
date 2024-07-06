using CongestionTaxCalculator.Domain.Common.Models;

namespace CongestionTaxCalculator.Domain.City.ValueObjects;

public sealed class TaxRulesId : ValueObject
{
    public Guid Value { get; }

    private TaxRulesId(Guid value)
    {
        Value = value;
    }

    public static TaxRulesId Create(Guid value)
    {
        return new TaxRulesId(value);
    }

    public static TaxRulesId CreateUnique()
    {
        return new TaxRulesId(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}