using CongestionTaxCalculator.Domain.Common.Models;

namespace CongestionTaxCalculator.Domain.City.ValueObjects;

public class Vehicle : ValueObject
{
    public string Name { get;}

    private Vehicle(string name)
    {
        Name = name;
    }

    public static Vehicle Create(string name)
    {
        return new Vehicle(name);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}