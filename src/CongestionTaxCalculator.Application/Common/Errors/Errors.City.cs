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

        public static Error InvalidYear => Error.Validation(
            code: "City.InvalidYear",
            description: "At least one of the selected date years is not valid.");   
        
        public static Error MoreThanOneYearCalculation=> Error.Validation(
            code: "City.MoreThanOneYearCalculation",
            description: "Sorry, we cannot calculate tax for dates not in a year.");

        public static Error TaxRulesNotFound => Error.Validation(
            code: "City.TaxRulesNotFound",
            description: "Sorry, we do not have tax rules for selected dates.");
    }
}