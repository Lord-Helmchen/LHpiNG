using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LHpiNG.db.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "LHpi");

            migrationBuilder.CreateTable(
                name: "Expansions",
                schema: "LHpi",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    IdExpansion = table.Column<int>(nullable: true),
                    Abbreviation = table.Column<string>(nullable: true),
                    ReleaseDate = table.Column<string>(nullable: true),
                    IsReleased = table.Column<bool>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    ProductCount = table.Column<int>(nullable: true),
                    ReleaseDateTime = table.Column<DateTime>(nullable: true),
                    UrlSuffix = table.Column<string>(nullable: true),
                    ProductsUrlSuffix = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expansions", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "LHpi",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    ExpansionName = table.Column<string>(nullable: false),
                    IdProduct = table.Column<int>(nullable: true),
                    IdMetaproduct = table.Column<int>(nullable: true),
                    CountReprints = table.Column<int>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    Number = table.Column<int>(nullable: true),
                    Rarity = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => new { x.Name, x.ExpansionName });
                    table.ForeignKey(
                        name: "FK_Products_Expansions_ExpansionName",
                        column: x => x.ExpansionName,
                        principalSchema: "LHpi",
                        principalTable: "Expansions",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PriceGuides",
                schema: "LHpi",
                columns: table => new
                {
                    Uid = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Sell = table.Column<decimal>(nullable: false),
                    Low = table.Column<decimal>(nullable: false),
                    LowexPlus = table.Column<decimal>(nullable: false),
                    LowFoil = table.Column<decimal>(nullable: false),
                    Avg = table.Column<decimal>(nullable: false),
                    Trend = table.Column<decimal>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    IdProduct = table.Column<int>(nullable: true),
                    GermanProLow = table.Column<decimal>(nullable: true),
                    Suggested = table.Column<decimal>(nullable: true),
                    FoilSell = table.Column<decimal>(nullable: true),
                    FoilTrend = table.Column<decimal>(nullable: true),
                    FetchDate = table.Column<DateTime>(nullable: true),
                    PreviousPriceGuideUid = table.Column<int>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    ExpansionName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceGuides", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_PriceGuides_PriceGuides_PreviousPriceGuideUid",
                        column: x => x.PreviousPriceGuideUid,
                        principalSchema: "LHpi",
                        principalTable: "PriceGuides",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PriceGuides_Products_ProductName_ExpansionName",
                        columns: x => new { x.ProductName, x.ExpansionName },
                        principalSchema: "LHpi",
                        principalTable: "Products",
                        principalColumns: new[] { "Name", "ExpansionName" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceGuides_PreviousPriceGuideUid",
                schema: "LHpi",
                table: "PriceGuides",
                column: "PreviousPriceGuideUid",
                unique: true,
                filter: "[PreviousPriceGuideUid] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PriceGuides_ProductName_ExpansionName",
                schema: "LHpi",
                table: "PriceGuides",
                columns: new[] { "ProductName", "ExpansionName" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ExpansionName",
                schema: "LHpi",
                table: "Products",
                column: "ExpansionName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceGuides",
                schema: "LHpi");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "LHpi");

            migrationBuilder.DropTable(
                name: "Expansions",
                schema: "LHpi");
        }
    }
}
