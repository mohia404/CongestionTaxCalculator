using CongestionTaxCalculator.Domain.Common.Exceptions;

namespace CongestionTaxCalculator.Domain.City.Exceptions;

public class TaxRulesNotFoundException : ValidationException
{
    public TaxRulesNotFoundException()
    {
    }
}