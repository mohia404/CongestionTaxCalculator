using CongestionTaxCalculator.Domain.City.Entities;
using CongestionTaxCalculator.Domain.City.Exceptions;
using CongestionTaxCalculator.Domain.City.ValueObjects;
using CongestionTaxCalculator.Domain.Common.Models;

namespace CongestionTaxCalculator.Domain.City;

public class City : AggregateRoot<CityId, int>
{
    public string Name { get; set; }

    private List<TaxRulesPerYear> taxRulesPerYears = [];
    public IReadOnlyList<TaxRulesPerYear> TaxRulesPerYears => taxRulesPerYears.AsReadOnly();

    private City(CityId id, string name, List<TaxRulesPerYear> taxRulesPerYears) : base(id)
    {
        Name = name;
        this.taxRulesPerYears = taxRulesPerYears;
    }

    public static City Create(CityId id, string name, List<TaxRulesPerYear> taxRulesPerYears)
    {
        return new City(id, name, taxRulesPerYears);
    }

    public int GetTax(Vehicle vehicle, DateTime[] datePassesToll)
    {
        var validYear = 2013;
        if (datePassesToll.Any(x => x.Year != validYear))
            throw new InvalidYearException();

        var taxRulesPerYear = TaxRulesPerYears.FirstOrDefault(x => x.Year == validYear)
            ?? throw new TaxRulesNotFoundException();

        if (taxRulesPerYear.IsVehicleTaxFree(vehicle))
            return 0;

        var totalTax = 0;

        if(datePassesToll is null || datePassesToll.Length == 0)
            return 0;

        var benchmarkDate = datePassesToll[0];

        datePassesToll = [.. datePassesToll.OrderBy(x => x.Date)];

        for (int i = 0; i < datePassesToll.Length; i++)
        {
            if (taxRulesPerYear.IsTaxFreeDay(datePassesToll[i]))
                break;

            var tax = taxRulesPerYear.GetFixedTimeTaxAmount(TimeOnly.FromDateTime(datePassesToll[i]));

            totalTax += tax;

            if (i+1 < datePassesToll.Length && (datePassesToll[i + 1] - benchmarkDate).TotalHours <= 1)
            {
                var nextTimeTax = taxRulesPerYear.GetFixedTimeTaxAmount(TimeOnly.FromDateTime(datePassesToll[i+1]));

                if(nextTimeTax > tax)
                    totalTax += nextTimeTax - tax;

                i++;
            }
            else
            {
                benchmarkDate = datePassesToll[i];
            }
        }

        return totalTax;
    }

#pragma warning disable CS8618
    private City() { }
#pragma warning restore CS8618
}