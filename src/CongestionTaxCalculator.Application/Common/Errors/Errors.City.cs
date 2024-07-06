using ErrorOr;

namespace CongestionTaxCalculator.Application.Common.Errors;

public static partial class Errors
{
    public static class City
    {
        public static Error NotFound => Error.NotFound(
            code: "City.NotFound",
            description: "City not found.");

        public static Error VehicleNotValid => Error.Validation(
            code: "City.VehicleNotValid",
            description: "Vehicle is not valid.");
    }
}