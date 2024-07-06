using ErrorOr;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CongestionTaxCalculator.Api.Endpoints;

public static class ErrorExtensions
{
    public static IResult ToProblemResult(this List<Error> errors)
    {
        if (errors.Count is 0)
        {
            return TypedResults.Problem();
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return errors.ToValidationProblemResult();
        }

        return errors[0].ToProblemResult();
    }

    private static ProblemHttpResult ToProblemResult(this Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError,
        };

        return TypedResults.Problem(statusCode: statusCode, title: error.Description);
    }

    private static ValidationProblem ToValidationProblemResult(this List<Error> errors)
    {
        Dictionary<string, string[]> validationErrors = errors
            .GroupBy(error => error.Code)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.Description).ToArray());

        return TypedResults.ValidationProblem(validationErrors);
    }
}