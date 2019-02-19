using Microsoft.EntityFrameworkCore.Migrations;

namespace LHpiNG.db.Migrations
{
    public partial class ChangeObjectRarityType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Rarity",
                schema: "LHpi",
                table: "AlbumObjects",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Rarity",
                schema: "LHpi",
                table: "AlbumObjects",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
