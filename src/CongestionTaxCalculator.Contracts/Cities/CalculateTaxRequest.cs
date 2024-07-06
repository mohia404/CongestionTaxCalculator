namespace CongestionTaxCalculator.Contracts.Cities
{
    public record CalculateTaxRequest(string CityName, string VehicleName, DateTime[] DatePassesToll);
}