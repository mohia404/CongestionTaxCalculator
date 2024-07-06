using CongestionTaxCalculator.Domain.City;
using CongestionTaxCalculator.Domain.City.Entities;
using CongestionTaxCalculator.Domain.City.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CongestionTaxCalculator.Infrastructure.Data.Config;

public class CityConfig : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        ConfigureCitiesTable(builder);
        ConfigureTaxRulesTable(builder);
    }

    private void ConfigureCitiesTable(EntityTypeBuilder<City> builder)
    {
        builder
            .ToTable("Cities");

        builder
            .HasKey(s => s.Id);

        builder
            .Property(d => d.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => CityId.Create(value));

        builder
            .Property(s => s.Name)
            .HasMaxLength(100);
    }

    private static void ConfigureTaxRulesTable(EntityTypeBuilder<City> builder)
    {
        builder.OwnsMany(x => x.TaxRulesPerYears, taxRulesBuilder =>
        {
            taxRulesBuilder
                .ToTable("TaxRulesPerYears");

            taxRulesBuilder
                .WithOwner()
                .HasForeignKey("CityId");

            taxRulesBuilder.HasKey(nameof(TaxRulesPerYear.Id));

            taxRulesBuilder
                .Property(p => p.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => TaxRulesId.Create(value));

            taxRulesBuilder
                .Property(s => s.Year)
                .IsRequired();

            taxRulesBuilder.Property(e => e.TaxFreeDays)
            .HasConversion(
                v => string.Join(',', v.Select(date => date.ToString())),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                      .Select(date => DateTime.Parse(date)).ToArray())
            .HasColumnType("varchar(max)");

            taxRulesBuilder.OwnsMany(y => y.TaxFreeVehicles, ConfigureTaxFreeVehiclesTable);
            taxRulesBuilder.OwnsMany(y => y.FixedCongestionTaxAmounts, ConfigureFixedCongestionTaxAmountsTable);

        });

        builder.Metadata
            .FindNavigation(nameof(City.TaxRulesPerYears))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigureTaxFreeVehiclesTable(OwnedNavigationBuilder<TaxRulesPerYear, Vehicle> builder)
    {
        builder
            .WithOwner()
            .HasForeignKey("TaxRulesId");

        builder
            .ToTable("TaxFreeVehicles");

        builder.HasKey(nameof(Vehicle.Name), "TaxRulesId");
    }

    private static void ConfigureFixedCongestionTaxAmountsTable(OwnedNavigationBuilder<TaxRulesPerYear, FixedCongestionTaxAmount> builder)
    {
        builder
            .WithOwner()
            .HasForeignKey("TaxRulesId");

        builder
            .ToTable("FixedCongestionTaxAmounts");

        builder
            .Property(s => s.TaxAmount)
            .IsRequired();

        builder.HasKey(nameof(FixedCongestionTaxAmount.FromTime), nameof(FixedCongestionTaxAmount.ToTime), "TaxRulesId");
    }
}