namespace CongestionTaxCalculator.Domain.City.ValueObjects;

public class FixedCongestionTaxAmount
{
    public TimeOnly FromTime { get; set; }
    public TimeOnly ToTime { get; set; }
    public int TaxAmount { get; set; }
}