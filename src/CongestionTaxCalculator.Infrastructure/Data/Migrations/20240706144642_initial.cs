using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CongestionTaxCalculator.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxRulesPerYears",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    TaxFreeDays = table.Column<string>(type: "varchar(max)", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxRulesPerYears", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxRulesPerYears_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FixedCongestionTaxAmounts",
                columns: table => new
                {
                    FromTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    ToTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    TaxRulesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaxAmount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixedCongestionTaxAmounts", x => new { x.FromTime, x.ToTime, x.TaxRulesId });
                    table.ForeignKey(
                        name: "FK_FixedCongestionTaxAmounts_TaxRulesPerYears_TaxRulesId",
                        column: x => x.TaxRulesId,
                        principalTable: "TaxRulesPerYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaxFreeVehicles",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TaxRulesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxFreeVehicles", x => new { x.Name, x.TaxRulesId });
                    table.ForeignKey(
                        name: "FK_TaxFreeVehicles_TaxRulesPerYears_TaxRulesId",
                        column: x => x.TaxRulesId,
                        principalTable: "TaxRulesPerYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FixedCongestionTaxAmounts_TaxRulesId",
                table: "FixedCongestionTaxAmounts",
                column: "TaxRulesId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxFreeVehicles_TaxRulesId",
                table: "TaxFreeVehicles",
                column: "TaxRulesId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxRulesPerYears_CityId",
                table: "TaxRulesPerYears",
                column: "CityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FixedCongestionTaxAmounts");

            migrationBuilder.DropTable(
                name: "TaxFreeVehicles");

            migrationBuilder.DropTable(
                name: "TaxRulesPerYears");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
