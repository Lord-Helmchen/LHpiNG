using Microsoft.EntityFrameworkCore.Migrations;

namespace LHpiNG.db.Migrations
{
    public partial class MakePricesNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Trend",
                schema: "LHpi",
                table: "PriceGuides",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Sell",
                schema: "LHpi",
                table: "PriceGuides",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "LowexPlus",
                schema: "LHpi",
                table: "PriceGuides",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "LowFoil",
                schema: "LHpi",
                table: "PriceGuides",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Low",
                schema: "LHpi",
                table: "PriceGuides",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Avg",
                schema: "LHpi",
                table: "PriceGuides",
                nullable: true,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Trend",
                schema: "LHpi",
                table: "PriceGuides",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Sell",
                schema: "LHpi",
                table: "PriceGuides",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "LowexPlus",
                schema: "LHpi",
                table: "PriceGuides",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "LowFoil",
                schema: "LHpi",
                table: "PriceGuides",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Low",
                schema: "LHpi",
                table: "PriceGuides",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Avg",
                schema: "LHpi",
                table: "PriceGuides",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }
    }
}
