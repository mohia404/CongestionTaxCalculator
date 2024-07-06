using CongestionTaxCalculator.Application.Common.Persistence;
using CongestionTaxCalculator.Infrastructure.Data;
using CongestionTaxCalculator.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CongestionTaxCalculator.Infrastructure;

public static class DependencyInjectionRegister
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddPersistence(configuration);

        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<AppDbContextSeed>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICityRepository, CityRepository>();

        return services;
    }
}