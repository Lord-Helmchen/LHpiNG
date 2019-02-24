using Microsoft.EntityFrameworkCore.Migrations;

namespace LHpiNG.db.Migrations
{
    public partial class AddRarityEnumToAlbumObjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Rarity",
                schema: "LHpi",
                table: "AlbumObjects",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RarityString",
                schema: "LHpi",
                table: "AlbumObjects",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RarityString",
                schema: "LHpi",
                table: "AlbumObjects");

            migrationBuilder.AlterColumn<string>(
                name: "Rarity",
                schema: "LHpi",
                table: "AlbumObjects",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
