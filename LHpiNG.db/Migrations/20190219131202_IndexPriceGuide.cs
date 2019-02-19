using Microsoft.EntityFrameworkCore.Migrations;

namespace LHpiNG.db.Migrations
{
    public partial class IndexPriceGuide : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PriceGuides_ProductName_ExpansionName",
                schema: "LHpi",
                table: "PriceGuides");

            migrationBuilder.CreateIndex(
                name: "IX_PriceGuides_ProductName_ExpansionName_FetchDate",
                schema: "LHpi",
                table: "PriceGuides",
                columns: new[] { "ProductName", "ExpansionName", "FetchDate" },
                unique: true,
                filter: "FetchDate is not null and ProductName is not null and ExpansionName is not null");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PriceGuides_ProductName_ExpansionName_FetchDate",
                schema: "LHpi",
                table: "PriceGuides");

            migrationBuilder.CreateIndex(
                name: "IX_PriceGuides_ProductName_ExpansionName",
                schema: "LHpi",
                table: "PriceGuides",
                columns: new[] { "ProductName", "ExpansionName" });
        }
    }
}
