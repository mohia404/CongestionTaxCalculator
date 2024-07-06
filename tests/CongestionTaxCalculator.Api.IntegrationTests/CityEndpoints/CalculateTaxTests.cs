using CongestionTaxCalculator.Contracts.Cities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;

namespace CongestionTaxCalculator.Api.IntegrationTests.CityEndpoints;

public class CalculateTaxTests(CongestionTaxCalculatorApiFactory apiFactory) : IClassFixture<CongestionTaxCalculatorApiFactory>
{
    [Fact]
    public async Task CalculateTax_ShouldReturnValidTax_WhenInputsAreValid()
    {
        // arrange
        var client = apiFactory.CreateClient();
        var request = new CalculateTaxRequest("Gothenburg", "AnyCar", [new DateTime(2013, 5, 15, 15, 5, 0)]);

        // act
        var response = await client.PostAsJsonAsync("api/cities/calculate-tax", request);

        // assert
        var customerResponse = await response.Content.ReadFromJsonAsync<CalculateTaxResponse>();
        customerResponse?.Tax.Should().Be(13);
    }

    [Fact]
    public async Task CalculateTax_ShouldReturnCityNotFound_WhenCityIsNotGothenburg()
    {
        // arrange
        var client = apiFactory.CreateClient();
        var request = new CalculateTaxRequest("AnyCity", "AnyCar", [new DateTime(2013, 5, 15, 15, 5, 0)]);

        // act
        var response = await client.PostAsJsonAsync("api/cities/calculate-tax", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        problem!.Title.Should().Be("City not found.");
    }

    [Fact]
    public async Task CalculateTax_ShouldReturnYearNotValid_WhenInputDatesAreEmpty()
    {
        // arrange
        var client = apiFactory.CreateClient();
        var request = new CalculateTaxRequest("Gothenburg", "AnyCar", null);

        // act
        var response = await client.PostAsJsonAsync("api/cities/calculate-tax", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        problem!.Title.Should().Be("One or more validation errors occurred.");
        problem!.Errors["City.InvalidYear"][0].Should().Be("At least one of the selected date years is not valid.");
    }

    [Fact]
    public async Task CalculateTax_ShouldReturnMoreThanOneYearCalculationError_WhenInputDatesAreInMoreThanOneYear()
    {
        // arrange
        var client = apiFactory.CreateClient();
        var request = new CalculateTaxRequest("Gothenburg", "AnyCar", [new DateTime(2013, 5, 15, 15, 5, 0), new DateTime(2014, 5, 15, 15, 5, 0)]);

        // act
        var response = await client.PostAsJsonAsync("api/cities/calculate-tax", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        problem!.Title.Should().Be("One or more validation errors occurred.");
        problem!.Errors["City.MoreThanOneYearCalculation"][0].Should().Be("Sorry, we cannot calculate tax for dates not in a year.");
    }

    [Fact]
    public async Task CalculateTax_ShouldReturnTaxRulesNotFound_WhenYearIsNot2013()
    {
        // arrange
        var client = apiFactory.CreateClient();
        var request = new CalculateTaxRequest("Gothenburg", "AnyCar", [new DateTime(2014, 5, 15, 15, 5, 0)]);

        // act
        var response = await client.PostAsJsonAsync("api/cities/calculate-tax", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        problem!.Title.Should().Be("Sorry, we do not have tax rules for selected dates.");
    }

    [Fact]
    public async Task CalculateTax_ShouldReturnZeroTax_WhenTheVehicleIsFreeTaxVehicle()
    {
        // arrange
        var client = apiFactory.CreateClient();
        var request = new CalculateTaxRequest("Gothenburg", "Emergency vehicle", [new DateTime(2013, 5, 15, 15, 5, 0)]);

        // act
        var response = await client.PostAsJsonAsync("api/cities/calculate-tax", request);

        // assert
        var customerResponse = await response.Content.ReadFromJsonAsync<CalculateTaxResponse>();
        customerResponse?.Tax.Should().Be(0);
    }

    [Fact]
    public async Task CalculateTax_ShouldReturnZeroTax_WhenTheDayIsTaxFree()
    {
        // arrange
        var client = apiFactory.CreateClient();
        var request = new CalculateTaxRequest("Gothenburg", "AnyCar", [new DateTime(2013, 5, 5, 15, 5, 0)]);

        // act
        var response = await client.PostAsJsonAsync("api/cities/calculate-tax", request);

        // assert
        var customerResponse = await response.Content.ReadFromJsonAsync<CalculateTaxResponse>();
        customerResponse?.Tax.Should().Be(0);
    }
}