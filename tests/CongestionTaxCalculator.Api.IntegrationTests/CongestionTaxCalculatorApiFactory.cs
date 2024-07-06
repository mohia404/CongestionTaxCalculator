using CongestionTaxCalculator.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MsSql;

namespace CongestionTaxCalculator.Api.IntegrationTests;

public class CongestionTaxCalculatorApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly MsSqlContainer sqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .Build();

    public async Task InitializeAsync()
    {
        await sqlContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
            services.RemoveAll(typeof(DbContext));
            services.RemoveAll(typeof(AppDbContextSeed));

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(sqlContainer.GetConnectionString());
            });
            services.AddScoped<AppDbContextSeed>();
        });
    }

    public new async Task DisposeAsync()
    {
        await sqlContainer.StopAsync();
    }
}