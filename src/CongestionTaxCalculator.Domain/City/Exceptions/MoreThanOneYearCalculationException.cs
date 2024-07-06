using CongestionTaxCalculator.Domain.Common.Exceptions;

namespace CongestionTaxCalculator.Domain.City.Exceptions;

public class MoreThanOneYearCalculationException : ValidationException
{
    public MoreThanOneYearCalculationException()
    {
    }
}