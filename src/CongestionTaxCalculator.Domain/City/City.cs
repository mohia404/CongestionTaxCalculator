using CongestionTaxCalculator.Domain.City.Entities;
using CongestionTaxCalculator.Domain.City.ValueObjects;

namespace CongestionTaxCalculator.Domain.City;

public class City
{
    public string Name { get; set; }

    public TaxRules TaxRules { get; set; }

    public int GetTax(Vehicle vehicle, DateTime[] datePassesToll)
    {
        if (TaxRules.IsVehicleTaxFree(vehicle))
            return 0;

        var totalTax = 0;

        if(datePassesToll is null || datePassesToll.Length == 0)
            return 0;

        var benchmarkDate = datePassesToll[0];

        datePassesToll = [.. datePassesToll.OrderBy(x => x.Date)];

        for (int i = 0; i < datePassesToll.Length; i++)
        {
            if (TaxRules.IsTaxFreeDay(DateOnly.FromDateTime(datePassesToll[i])))
                break;


            var tax = TaxRules.GetFixedTimeTaxAmount(TimeOnly.FromDateTime(datePassesToll[i]));

            totalTax += tax;

            if (i+1 < datePassesToll.Length && (datePassesToll[i + 1] - benchmarkDate).TotalHours <= 1)
            {
                var nextTimeTax = TaxRules.GetFixedTimeTaxAmount(TimeOnly.FromDateTime(datePassesToll[i+1]));

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
}