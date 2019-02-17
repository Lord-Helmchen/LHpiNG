using Microsoft.EntityFrameworkCore.Migrations;

namespace LHpiNG.db.Migrations
{
    public partial class RemovePreviousPriceGuideColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceGuides_PriceGuides_PreviousPriceGuideUid",
                schema: "LHpi",
                table: "PriceGuides");

            migrationBuilder.DropIndex(
                name: "IX_PriceGuides_PreviousPriceGuideUid",
                schema: "LHpi",
                table: "PriceGuides");

            migrationBuilder.DropColumn(
                name: "PreviousPriceGuideUid",
                schema: "LHpi",
                table: "PriceGuides");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PreviousPriceGuideUid",
                schema: "LHpi",
                table: "PriceGuides",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PriceGuides_PreviousPriceGuideUid",
                schema: "LHpi",
                table: "PriceGuides",
                column: "PreviousPriceGuideUid",
                unique: true,
                filter: "[PreviousPriceGuideUid] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceGuides_PriceGuides_PreviousPriceGuideUid",
                schema: "LHpi",
                table: "PriceGuides",
                column: "PreviousPriceGuideUid",
                principalSchema: "LHpi",
                principalTable: "PriceGuides",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
