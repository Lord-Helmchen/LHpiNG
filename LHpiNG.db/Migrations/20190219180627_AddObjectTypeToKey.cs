using Microsoft.EntityFrameworkCore.Migrations;

namespace LHpiNG.db.Migrations
{
    public partial class AddObjectTypeToKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AlbumObjects",
                schema: "LHpi",
                table: "AlbumObjects");

            migrationBuilder.AddColumn<int>(
                name: "ObjectType",
                schema: "LHpi",
                table: "AlbumObjects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AlbumObjects",
                schema: "LHpi",
                table: "AlbumObjects",
                columns: new[] { "OracleName", "Version", "Set", "ObjectType", "Language" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AlbumObjects",
                schema: "LHpi",
                table: "AlbumObjects");

            migrationBuilder.DropColumn(
                name: "ObjectType",
                schema: "LHpi",
                table: "AlbumObjects");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AlbumObjects",
                schema: "LHpi",
                table: "AlbumObjects",
                columns: new[] { "OracleName", "Version", "Set", "Language" });
        }
    }
}
