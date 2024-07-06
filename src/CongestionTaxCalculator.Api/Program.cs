using CongestionTaxCalculator.Api.Endpoints;
using CongestionTaxCalculator.Application;
using CongestionTaxCalculator.Infrastructure;
using CongestionTaxCalculator.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var hostEnvironment = app.Services.GetService<IWebHostEnvironment>();
var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
var logger = loggerFactory.CreateLogger(nameof(app));
logger.LogInformation($"Starting in environment {hostEnvironment.EnvironmentName}");
try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var seedService = services.GetRequiredService<AppDbContextSeed>();
    await seedService.SeedAsync(DateTime.Now);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred seeding the DB.");
}

app.UseHttpsRedirection();

app.MapCityEndpoints();

app.Run();