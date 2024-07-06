using CongestionTaxCalculator.Domain.Common.Exceptions;

namespace CongestionTaxCalculator.Domain.City.Exceptions;

public class InvalidYearException : ValidationException
{
    public InvalidYearException()
    {
    }
}